using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class LoansController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LoansController(LibraryContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        [HttpGet]
        public IActionResult GetLoans()
        {
            try
            {
                //mielőtt lekérjük a kölcsönzéseket, aktualizáljuk a listát, azaz az inaktív múltbeli kölcsönzések törlésre kerülnek
                /*foreach(var loan in _context.Loans)
                {
                    if (loan.LastDay < DateTime.Now && !loan.IsActive)
                    {
                        Debug.WriteLine("loan last day: " + loan.LastDay);
                        Debug.WriteLine("<: " + (loan.LastDay < DateTime.Now));
                        _context.Loans.Remove(loan);
                    }
                }*/

                var c = _context.Loans.Count();
                for (int i = 1; i <= c; ++i)
                {
                    if (_context.Loans.Select(l => l.Id).Contains(i))
                    {
                        if (_context.Loans.FirstOrDefault(l => l.Id == i).LastDay < DateTime.Now && !_context.Loans.FirstOrDefault(l => l.Id == i).IsActive)
                        {
                            _context.Loans.Remove(_context.Loans.FirstOrDefault(l => l.Id == i));
                        }
                    }
                }

                _context.SaveChanges();

                var ret = _context.Loans
                    .ToList()
                    .Select(loan => new LoanDTO
                    {
                        Id = loan.Id,
                        TomeId = loan.TomeId,
                        FirstDay = loan.FirstDay,
                        LastDay = loan.LastDay,
                        IsActive = loan.IsActive,
                        UserId = loan.UserId
                    });

                return Ok(ret);
               }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("{id}")]
        [Authorize(Roles = "administrator")]
        public IActionResult DeleteLoan(Int32 id)
        {
            try
            {
                Loan loan = _context.Loans.FirstOrDefault(l => l.Id == id);

                if (loan == null)
                    return NotFound();

                _context.Loans.Remove(loan);
                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("book/{id}")]
        public IActionResult GetLoansForBook(Int32 bookId)
        {
            try
            {
                //ez a mock teszttel nem működik
                /*foreach (var loan in _context.Loans)
                {
                    if (loan.LastDay < DateTime.Now && !loan.IsActive)
                    {
                        _context.Loans.Remove(loan);
                    }
                }*/

                var c = _context.Loans.Count();
                for (int i=1; i <= c; ++i)
                {
                    if (_context.Loans.Select(l => l.Id).Contains(i))
                    {
                        if (_context.Loans.FirstOrDefault(l => l.Id == i).LastDay < DateTime.Now && !_context.Loans.FirstOrDefault(l => l.Id == i).IsActive)
                        {
                            _context.Loans.Remove(_context.Loans.FirstOrDefault(l => l.Id == i));
                        }
                    }
                }

                _context.SaveChanges();

                var returnval = _context.Loans
                    .Include(l => l.Tome)
                    .Include(l => l.Tome.Book)
                    .Where(l => l.Tome.BookId == bookId)
                    .ToList()
                    .Select(loan => new LoanDTO
                    {
                        Id = loan.Id,
                        TomeId = loan.TomeId,
                        FirstDay = loan.FirstDay,
                        LastDay = loan.LastDay,
                        IsActive = loan.IsActive,
                        UserId = loan.UserId
                    });

                return Ok(returnval);

                /*return Ok(_context.Loans
                    .Include(l => l.Tome)
                    .Include(l => l.Tome.Book)
                    .Where(l => l.Tome.BookId == bookId)
                    .ToList()
                    .Select(loan => new LoanDTO
                    {
                        Id = loan.Id,
                        TomeId = loan.TomeId,
                        FirstDay = loan.FirstDay,
                        LastDay = loan.LastDay,
                        IsActive = loan.IsActive,
                        UserId = loan.UserId
                    }));*/
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("tome/{id}")]
        public IActionResult GetLoansForTome(Int32 tomeId)
        {
            try
            {
                /*
                foreach (var loan in _context.Loans)
                {
                    if (loan.LastDay < DateTime.Now && !loan.IsActive)
                    {
                        _context.Loans.Remove(loan);
                    }
                }*/

                var c = _context.Loans.Count();
                for (int i = 1; i <= c; i++)
                {
                    if (_context.Loans.Select(l => l.Id).Contains(i))
                    {
                        if (_context.Loans.FirstOrDefault(l => l.Id == i).LastDay < DateTime.Now && !_context.Loans.FirstOrDefault(l => l.Id == i).IsActive)
                        {
                            _context.Loans.Remove(_context.Loans.FirstOrDefault(l => l.Id == i));
                        }
                    }
                }

                _context.SaveChanges();

                var ret = _context.Loans
                    .Include(l => l.Tome)
                    .Include(l => l.Tome.Book)
                    .Where(l => l.Tome.Id == tomeId)
                    .ToList()
                    .Select(loan => new LoanDTO
                    {
                        Id = loan.Id,
                        TomeId = loan.TomeId,
                        FirstDay = loan.FirstDay,
                        LastDay = loan.LastDay,
                        IsActive = loan.IsActive,
                        UserId = loan.UserId
                    });

                return Ok(ret);

                /*return Ok(_context.Loans
                    .Include(l => l.Tome)
                    .Include(l => l.Tome.Book)
                    .Where(l => l.Tome.Id == tomeId)
                    .ToList()
                    .Select(loan => new LoanDTO
                    {
                        Id = loan.Id,
                        TomeId = loan.TomeId,
                        FirstDay = loan.FirstDay,
                        LastDay = loan.LastDay,
                        IsActive = loan.IsActive,
                        UserId = loan.UserId
                    }));*/
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Authorize(Roles = "administrator")]
        public IActionResult PutLoan([FromBody] LoanDTO loanDTO)
        {
            try
            {
                Loan loan = _context.Loans.FirstOrDefault(b => b.Id == loanDTO.Id);

                if (loan == null)
                    return NotFound();

                loan.TomeId = loanDTO.TomeId;
                loan.FirstDay = loanDTO.FirstDay;
                loan.LastDay = loanDTO.LastDay;
                loan.IsActive = loanDTO.IsActive;
                    
                _context.SaveChanges();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}