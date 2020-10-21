using Library.Admin.Persistence;
using Library.Data;
using Library.Model;
using Library.Service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Library.Service.Test
{
   /* public class ControllerTests : IDisposable
    {
        private readonly LibraryContext _context;
        private readonly BooksController _booksController;

        public ControllerTests()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _context = new LibraryContext(options);
            TestDbInitializer.Initialize(_context);
            _booksController = new BooksController(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void PostBookTest()
        {
            // Arrange
            var newList = new BookDTO { Title = "New test list" };
            var count = _context.Books.Count();

            // Act
            var result = _booksController.PostBook(newList);

            // Assert
            var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            var content = Assert.IsAssignableFrom<BookDTO>(objectResult.Value);
            Assert.Equal(count + 1, _context.Books.Count());
        }
    }*/
}
