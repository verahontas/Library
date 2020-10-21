using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models
{
    public interface ILoanService
    {
        IEnumerable<Book> Books { get; }

        IEnumerable<Tome> Tomes { get; }

        IEnumerable<Loan> Loans { get; }

        Book GetBook(int? bookId);

        bool AddBook(Book book);

        bool RemoveTome(int? tomeId);

        bool AddTome(Tome tome);

        Loan GetLoan(int? loanId);

        bool ChangeLoanStatus(int? loanId);

        IEnumerable<Book> GetBooks();

        IEnumerable<int> GetBookImageIds(int? bookId);

        Byte[] GetBookImage(int? bookId);

        Byte[] GetBookImage(int? bookId, bool large);

        Tome GetTome(int? tomeId);

        LoanViewModel NewLoan(int? tomeId);

        Task<Boolean> SaveLoanAsync(Int32? tomeId, String userName, LoanViewModel rent);

        LoanDateError ValidateLoan(DateTime start, DateTime end, int? tomeId);

        IEnumerable<DateTime> GetLoanDates(Int32 tomeId, Int32 year, Int32 month);

    }
}
