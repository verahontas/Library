using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Library.Data;
using Library.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Library.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            try
            {
                return Ok(_context.Books
                    .Include(b => b.Tomes)
                    .ToList()
                    .Select(book => new BookDTO
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Author = book.Author,
                        ISBN = book.ISBN,
                        Year = book.Year,
                        NumberOfLoans = book.NumberOfLoans
                    }));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetBook(Int32 id)
        {
            try
            {
                var book = _context.Books
                    .Include(b => b.Tomes)
                    .Single(b => b.Id == id);
                return Ok(new BookDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    ISBN = book.ISBN,
                    Year = book.Year,
                    NumberOfLoans = book.NumberOfLoans
                });
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public IActionResult PostBook([FromBody] BookDTO bookDTO)
        {
            Book book = new Book
            {
                Title = bookDTO.Title,
                Author = bookDTO.Author,
                ISBN = bookDTO.ISBN,
                Year = bookDTO.Year,
                NumberOfLoans = bookDTO.NumberOfLoans
            };

            _context.Books.Add(book);

            _context.SaveChanges();

            try
            {
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDTO);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /*
        [HttpPut]
        [Authorize(Roles = "administrator")]
        public IActionResult PutBook([FromBody] BookDTO bookDTO)
        {
            try
            {
                Book book = _context.Books.FirstOrDefault(b => b.Id == bookDTO.Id);

                if (book == null)
                    return NotFound();

                book.Title = bookDTO.Title;
                book.Author = bookDTO.Author;
                book.Year = bookDTO.Year;
                book.ISBN = bookDTO.ISBN;
                book.NumberOfLoans = bookDTO.NumberOfLoans;

                _context.SaveChanges();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator")]
        public IActionResult DeleteBook(Int32 id)
        {
            try
            {
                Book book = _context.Books.FirstOrDefault(b => b.Id == id);

                if (book == null)
                    return NotFound();

                _context.Books.Remove(book);

                _context.SaveChanges();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        */
    }
}