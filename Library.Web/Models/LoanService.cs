using Library.Controllers;
using Library.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models
{
    public class LoanService : ILoanService
    {
        private readonly LibraryContext _context;
        private readonly LoanDateValidator _loanDateValidator;
        private readonly UserManager<Guest> _userManager;

        public LoanService(LibraryContext context, UserManager<Guest> userManager)
        {
            _context = context;
            _userManager = userManager;
            _loanDateValidator = new LoanDateValidator(_context);
        }

        public IEnumerable<Book> Books => _context.Books.Include(l => l.Tomes);

        public IEnumerable<Tome> Tomes => _context.Tomes.Include(l => l.Loans).Include(l => l.Book);

        public IEnumerable<Loan> Loans => _context.Loans.Include(l => l.Tome).Include(l => l.Tome.Book);
     
        public Book GetBook(int? bookId)
        {
            if (bookId == null)
            {
                return null;
            }
            
            return _context.Books
                .Include(l => l.Tomes)
                .FirstOrDefault(l => l.Id == bookId);
        }

        public bool AddBook(Book book)
        {
            _context.Add(book);
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool RemoveTome(int? tomeId)
        {
            //ha van rá aktív kölcsönzés,nem tudjuk törölni
            foreach(var loan in _context.Loans)
            {
                if (loan.TomeId == tomeId && loan.IsActive)
                {
                    return false;
                }
            }

            var tome = _context.Tomes.FirstOrDefault(l => l.Id == tomeId);

            _context.Remove(tome);

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            //az összes erre leadott kölcsönzést is törölni kell
            foreach (var loan in _context.Loans)
            {
                if (loan.TomeId == tomeId)
                {
                    _context.Remove(loan);
                }
            }

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool AddTome(Tome tome)
        {
            _context.Add(tome);
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public Loan GetLoan(int? loanId)
        {
            if (loanId == null)
                return null;

            return _context.Loans
                .Include(l => l.Tome)
                .Include(l => l.Tome.Book)
                .FirstOrDefault(l => l.Id == loanId);
        }

        public bool ChangeLoanStatus(int? loanId)
        {
            if (loanId != null)
            {
                //megkeressük, hogy annak a kötetnek, amelyikre ez a kölcsönzés is van, 
                //van-e már aktív kölcsönzése, mert ha igen,
                //akkor nem regisztrálhatunk újabb aktív kölcsönzést
                var tomeId = _context.Loans.FirstOrDefault(l => l.Id == loanId).TomeId;

                foreach (var loan in _context.Loans)
                {
                    if (loan.TomeId == tomeId && loan.IsActive)
                    {
                        return false;
                    }
                }

                _context.Loans.FirstOrDefault(l => l.Id == loanId).IsActive = _context.Loans.FirstOrDefault(l => l.Id == loanId).IsActive ? false : true;
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public IEnumerable<Book> GetBooks()
        {
            return _context.Books.Include(l => l.Tomes);
        }

        public IEnumerable<int> GetBookImageIds(int? bookId)
        {
            if (bookId == null)
                return null;

            return _context.BookImages
                .Where(image => image.BookId == bookId)
                .Select(image => image.Id);
        }

        public Byte[] GetBookImage(int? bookId)
        {
            if (bookId == null)
            {
                return null;
            }

            return _context.BookImages
                .Where(l => l.BookId == bookId)
                .Select(l => l.ImageSmall)
                .FirstOrDefault();
        }

        public Byte[] GetBookImage(int? imageId, bool large)
        {
            if (imageId == null)
                return null;

            Byte[] imageContent = _context.BookImages
                .Where(image => image.Id == imageId)
                .Select(image => large ? image.ImageLarge : image.ImageSmall)
                .FirstOrDefault();

            return imageContent;
        }

        public Tome GetTome(int? tomeId)
        {
            if (tomeId == null)
                return null;

            Tome tome = _context.Tomes
                .Include(l => l.Book)
                .Include(l => l.Loans)
                .FirstOrDefault(l => l.Id == tomeId);

            return tome;
        }

        public LoanViewModel NewLoan(int? tomeId)
        {
            if (tomeId == null)
                return null;

            Tome tome = _context.Tomes
                .Include(l => l.Book)
                .Include(l => l.Loans)
                .FirstOrDefault(l => l.Id == tomeId);

            LoanViewModel loan = new LoanViewModel { Tome = tome };

            loan.LoanFirstDay = DateTime.Today;

            loan.LoanLastDay = loan.LoanFirstDay + TimeSpan.FromDays(7);

            return loan;
        }

        public async Task<Boolean> SaveLoanAsync(int? tomeId, string userName, LoanViewModel loan)
        {
            if (tomeId == null || loan == null)
                return false;

            if (!Validator.TryValidateObject(loan, new ValidationContext(loan, null, null), null))
                return false;

            if (_loanDateValidator.Validate(loan.LoanFirstDay, loan.LoanLastDay, tomeId.Value) != LoanDateError.None)
                return false;

            Guest guest = await _userManager.FindByNameAsync(userName);

            if (guest == null)
                return false;

            _context.Loans.Add(new Loan
            {
                TomeId = loan.Tome.Id,
                UserId = guest.Id,
                FirstDay = loan.LoanFirstDay,
                LastDay = loan.LoanLastDay,
                IsActive = false //a kölcsönzést egy könyvtárosnak kell aktívvá tennie
            });

            //Debug.WriteLine("tomeid: " + tomeId);
            //Debug.WriteLine("numberofloans: " + _context.Books.Include(b => b.Tomes).FirstOrDefault(b => b.Id == loan.Tome.BookId).NumberOfLoans);

            /*foreach(var tome in _context.Tomes)
            {
                if (tome.Id == loan.Tome.Id) //ha erre a kötetre volt a kölcsönzés
                {
                    var book = _context.Books.FirstOrDefault(b => b.Id == tome.BookId);
                    book.NumberOfLoans += 1;
                }
            }*/

            var book = _context.Books.FirstOrDefault(b => b.Id == loan.Tome.BookId);
            book.NumberOfLoans += 1;

            //Debug.WriteLine("numberofloans: " + _context.Books.FirstOrDefault(b => b.Id == loan.Tome.Id).NumberOfLoans);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public LoanDateError ValidateLoan(DateTime firstDay, DateTime lastDay, int? tomeId)
        {
            if (tomeId == null)
                return LoanDateError.None;

            return _loanDateValidator.Validate(firstDay, lastDay, tomeId.Value);
        }

        public IEnumerable<DateTime> GetLoanDates(Int32 tomeId, Int32 year, Int32 month)
        {
            List<DateTime> isAvailable = new List<DateTime>();

            DateTime firstDay = new DateTime(year, month, 1) - TimeSpan.FromDays(50);
            DateTime lastDay = new DateTime(year, month, 1) + TimeSpan.FromDays(80);

            if (lastDay < DateTime.Today)
                return isAvailable;

            List<Loan> possibleConflicts = _context.Loans.Where(r => r.TomeId == tomeId && r.FirstDay <= lastDay && r.LastDay >= firstDay).ToList();

            for (DateTime date = firstDay; date < lastDay; date += TimeSpan.FromDays(1))
            {
                if (date > DateTime.Today &&
                    possibleConflicts.All(r => !r.IsConflicting(date)))
                {
                    isAvailable.Add(date);
                }
            }

            return isAvailable;
        }
    }
}
