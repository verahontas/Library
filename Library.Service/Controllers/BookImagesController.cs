using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Service.Controllers
{
    [Route("api/images")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class BookImagesController : Controller
    {
        private readonly LibraryContext _context;

        public BookImagesController(LibraryContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        [HttpGet("{bookId}")]
        public IActionResult GetImage(Int32 bookId)
        {
            return Ok(_context.BookImages.Where(image => image.BookId == bookId).Select(image => new ImageDTO { Id = image.Id, ImageSmall = image.ImageSmall }));
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public IActionResult PostImage([FromBody] ImageDTO image)
        {
            if (image == null || !_context.Books.Any(book => image.BookId == book.Id))
                return NotFound();

            BookImage bookImage = new BookImage
            {
                BookId = image.BookId,
                ImageSmall = image.ImageSmall,
                ImageLarge = image.ImageLarge
            };

            _context.BookImages.Add(bookImage);

            try
            {
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetImage), new { id = bookImage.Id }, bookImage.Id);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("{id}")]
        [Authorize(Roles = "administrator")]
        public IActionResult DeleteImage(Int32 id)
        {
            BookImage image = _context.BookImages.FirstOrDefault(im => im.Id == id);

            if (image == null)
                return NotFound();

            try
            {
                _context.BookImages.Remove(image);
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