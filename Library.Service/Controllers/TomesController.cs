using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Library.Model;
using System.Diagnostics;

namespace Library.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class TomesController : ControllerBase
    {
        private readonly LibraryContext _context;

        public TomesController(LibraryContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        [HttpGet]
        public IActionResult GetTomes()
        {
            try
            {
                var book = _context.Books.Where(l => l.Id == 1).ToList();
                Debug.WriteLine(book[0].Title);
                Debug.WriteLine(_context.Books.Select(l => l.Title).ToString());
                Debug.WriteLine(_context.Books.Where(l => l.Id == 1).Select(l => l.Title));
                Debug.WriteLine(_context.Books.Where(l => l.Id == 1).Select(l => l.Title).ToString());

                return Ok(_context.Tomes
                    .Include(t => t.Loans)
                    .ToList()
                    .Select(tome => new TomeDTO
                    {
                        Id = tome.Id,
                        BookId = tome.BookId,
                        BookTitle = _context.Books.Where(l => l.Id == tome.BookId).ToList()[0].Title
                    })) ;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetTome(Int32 id)
        {
            try
            {
                var tome = _context.Tomes
                    .Include(t => t.Loans)
                    .Single(t => t.Id == id);
                return Ok(new TomeDTO
                {
                    Id = tome.Id,
                    BookId = tome.BookId,
                    BookTitle = _context.Books.Where(l => l.Id == tome.BookId).ToList()[0].Title
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

        [HttpGet("{id}")]
        public IActionResult GetTomesForBook(Int32 bookId)
        {
            try
            {
                return Ok(_context.Tomes
                    .Include(t => t.Loans)
                    .Where(t => t.BookId == bookId)
                    .ToList()
                    .Select(tome => new TomeDTO
                    {
                        Id = tome.Id,
                        BookId = tome.BookId,
                        BookTitle = _context.Books.Where(l => l.Id == tome.BookId).ToList()[0].Title
                    }));
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
        public IActionResult PostTome([FromBody] TomeDTO tomeDTO)
        {
            Tome tome = new Tome
            {
                BookId = tomeDTO.Book.Id
            };

            _context.Tomes.Add(tome);

            _context.SaveChanges();

            try
            {

                return CreatedAtAction(nameof(GetTome), new { id = tome.Id }, tomeDTO);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("{id}")]
        [Authorize(Roles = "administrator")]
        public IActionResult DeleteTome(Int32 id)
        {
            try
            {
                Tome tome = _context.Tomes.FirstOrDefault(t => t.Id == id);

                if (tome == null)
                    return NotFound();

                _context.Tomes.Remove(tome);
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