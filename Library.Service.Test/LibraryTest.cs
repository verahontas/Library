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
	public class LibraryTest// : IDisposable
	{
		/*
		private readonly LibraryContext _context;
		private readonly List<BookDTO> _bookDTOs;
		private readonly List<TomeDTO> _tomeDTOs;
		private readonly List<LoanDTO> _loanDTOs;

		public LibraryTest()
		{
			var options = new DbContextOptionsBuilder<LibraryContext>()
				.UseInMemoryDatabase("LibraryTest")
				.Options;

			_context = new LibraryContext(options);
			_context.Database.EnsureCreated();

			var bookData = new List<Book>
			{
				new Book
				{
					Id = 1,
					Title = "TESTBOOKTITLE1",
					Author = "TESTBOOKAUTHOR1",
					Year = 2001,
					ISBN = "3649531867",
					NumberOfLoans = 3
				},
				new Book
				{
					Id = 2,
					Title = "TESTBOOKTITLE2",
					Author = "TESTBOOKAUTHOR2",
					Year = 2002,
					ISBN = "3649519767",
					NumberOfLoans = 1
				},
				new Book
				{
					Id = 3,
					Title = "TESTBOOKTITLE3",
					Author = "TESTBOOKAUTHOR3",
					Year = 2003,
					ISBN = "1976431867",
					NumberOfLoans = 0
				},
				new Book
				{
					Id = 4,
					Title = "TESTBOOKTITLE4",
					Author = "TESTBOOKAUTHOR4",
					Year = 2004,
					ISBN = "9761531867",
					NumberOfLoans = 1
				}
			};

			_context.Books.AddRange(bookData);

			var tomeData = new List<Tome>
			{
				new Tome
				{
					Id = 1,
					Book = bookData[1],
					BookId = bookData[1].Id
				},
				new Tome
				{
					Id = 2,
					Book = bookData[0],
					BookId = bookData[0].Id
				},
				new Tome
				{
					Id = 3,
					Book = bookData[1],
					BookId = bookData[1].Id
				},
				new Tome
				{
					Id = 4,
					Book = bookData[2],
					BookId = bookData[2].Id
				}
			};

			_context.Tomes.AddRange(tomeData);

			var loanData = new List<Loan>
			{
				new Loan
				{
					Id = 1,
					Tome = tomeData[2],
					TomeId = tomeData[2].Id,
					FirstDay = new DateTime(2020, 03, 04),
					LastDay = new DateTime(2020, 03, 05),
					IsActive = false
				},
				new Loan
				{
					Id = 2,
					Tome = tomeData[0],
					TomeId = tomeData[0].Id,
					FirstDay = new DateTime(2020, 05, 29),
					LastDay = new DateTime(2020, 06, 1),
					IsActive = false
				},
				new Loan
				{
					Id = 3,
					Tome = tomeData[3],
					TomeId = tomeData[3].Id,
					FirstDay = new DateTime(2020, 05, 19),
					LastDay = new DateTime(2020, 05, 29),
					IsActive = true
				},
				new Loan
				{
					Id = 4,
					Tome = tomeData[2],
					TomeId = tomeData[2].Id,
					FirstDay = new DateTime(2020, 03, 06),
					LastDay = new DateTime(2020, 03, 16),
					IsActive = false
				}
			};

			_context.Loans.AddRange(loanData);

			_context.SaveChanges();

			_bookDTOs = bookData.Select(book => new BookDTO
			{
				Id = book.Id,
				Title = book.Title,
				Author = book.Author,
				Year = book.Year,
				ISBN = book.ISBN,
				NumberOfLoans = book.NumberOfLoans
			}).ToList();

			_tomeDTOs = tomeData.Select(tome => new TomeDTO
			{
				Id = tome.Id,
				Book = _bookDTOs.Single(book => book.Id == tome.BookId),
				BookId = _bookDTOs.Single(book => book.Id == tome.BookId).Id,
				BookTitle = bookData.FirstOrDefault(book => book.Id == tome.BookId).Title
			}).ToList();

			_loanDTOs = loanData.Select(loan => new LoanDTO
			{
				Id = loan.Id,
				Tome = _tomeDTOs.Single(tome => tome.Id == loan.TomeId),
				TomeId = _tomeDTOs.Single(tome => tome.Id == loan.TomeId).Id
			}).ToList();

		}

		public void Dispose()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}

		[Fact]
		public void GetBooksTest()
		{
			var controller = new BooksController(_context);
			var result = controller.GetBooks();

			var objectResult = Assert.IsType<OkObjectResult>(result);
			var model = Assert.IsAssignableFrom<IEnumerable<BookDTO>>(objectResult.Value);
			Assert.Equal(_bookDTOs, model);
		}

		[Fact]
		public void GetTomesTest()
		{
			var controller = new TomesController(_context);
			var result = controller.GetTomes();

			var objectResult = Assert.IsType<OkObjectResult>(result);
			var model = Assert.IsAssignableFrom<IEnumerable<TomeDTO>>(objectResult.Value);
			Assert.Equal(_tomeDTOs, model);
		}

		[Fact]
		public void GetLoansTest()
		{
			var controller = new LoansController(_context);
			var result = controller.GetLoans();

			var objectResult = Assert.IsType<OkObjectResult>(result);
			var model = Assert.IsAssignableFrom<IEnumerable<LoanDTO>>(objectResult.Value);
			Assert.Equal(_loanDTOs, model);
		}

		[Fact]
		public void CreateBookTest()
		{
			var newBook = new BookDTO
			{
				Title = "TestTitle1",
				Author = "TestAuthor1",
				Year = 2001,
				ISBN = "12345678910",
				NumberOfLoans = 1
			};

			var controller = new BooksController(_context);
			var result = controller.PostBook(newBook);

			var objectResult = Assert.IsType<CreatedAtActionResult>(result);
			var model = Assert.IsAssignableFrom<BookDTO>(objectResult.Value);
			Assert.Equal(_bookDTOs.Count + 1, _context.Books.Count());
			Assert.Equal(newBook, model);
		}

		[Fact]
		public void CreateTomeTest()
		{
			var newTome = new TomeDTO
			{
				Book = _bookDTOs[2],
				BookId = _bookDTOs[2].Id,
				BookTitle = _bookDTOs[2].Title
			};

			var controller = new TomesController(_context);
			var result = controller.PostTome(newTome);

			var objectResult = Assert.IsType<CreatedAtActionResult>(result);
			var model = Assert.IsAssignableFrom<TomeDTO>(objectResult.Value);
			Assert.Equal(_tomeDTOs.Count + 1, _context.Tomes.Count());
			Assert.Equal(newTome, model);
		}

		[Fact]
		public void DeleteTomeTest()
		{
			var controller = new TomesController(_context);
			int deletedId = _context.Tomes.First().Id;
			var result = controller.DeleteTome(deletedId);

			Assert.IsType<OkResult>(result);
			Assert.Equal(_tomeDTOs.Count - 1, _context.Tomes.Count());
			Assert.DoesNotContain(deletedId, _context.Tomes.Select(b => b.Id));
		}*/
	}
}
