using Library.Data;
using Library.Model;
using Library.Service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;

namespace Library.Service.Test
{
    
    public class LibraryMockTest
    {
        
        private readonly List<BookDTO> _bookDTOs;
        private readonly List<TomeDTO> _tomeDTOs;
        private readonly List<LoanDTO> _loanDTOs;

        private Mock<DbSet<Book>> _bookMock;
        private Mock<DbSet<Tome>> _tomeMock;
        private Mock<DbSet<Loan>> _loanMock;
        private Mock<LibraryContext> _entityMock;

        public LibraryMockTest()
        {
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

            var loanData = new List<Loan>
            {
                new Loan
                {
                    Id = 1,
                    Tome = tomeData[2],
                    TomeId = tomeData[2].Id,
                    FirstDay = DateTime.Now.AddDays(-2),
                    LastDay = DateTime.Now.AddDays(-1),
            IsActive = false
                },
                new Loan
                {
                    Id = 2,
                    Tome = tomeData[0],
                    TomeId = tomeData[0].Id,
                    FirstDay = DateTime.Now.AddDays(3),
                    LastDay = DateTime.Now.AddDays(7),
                    IsActive = false
                },
                new Loan
                {
                    Id = 3,
                    Tome = tomeData[3],
                    TomeId = tomeData[3].Id,
                    FirstDay = DateTime.Now.AddDays(1),
                    LastDay = DateTime.Now.AddDays(8),
                    IsActive = true
                },
                new Loan
                {
                    Id = 4,
                    Tome = tomeData[2],
                    TomeId = tomeData[2].Id,
                    FirstDay = DateTime.Now.AddDays(10),
                    LastDay = DateTime.Now.AddDays(13),
                    IsActive = false
                }
            };

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
                Book = _bookDTOs.FirstOrDefault(book => book.Id == tome.BookId),
                BookId = _bookDTOs.FirstOrDefault(book => book.Id == tome.BookId).Id,
                BookTitle = bookData.FirstOrDefault(book => book.Id == tome.BookId).Title
            }).ToList();

            _loanDTOs = loanData.Select(loan => new LoanDTO
            {
                Id = loan.Id,
                Tome = _tomeDTOs.FirstOrDefault(tome => tome.Id == loan.TomeId),
                TomeId = _tomeDTOs.FirstOrDefault(tome => tome.Id == loan.TomeId).Id,
                FirstDay = loan.FirstDay,
                LastDay = loan.LastDay,
                IsActive = loan.IsActive
            }).ToList();

            IQueryable<Book> queryableBookData = bookData.AsQueryable();
            _bookMock = new Mock<DbSet<Book>>();
            _bookMock.As<IQueryable<Book>>().Setup(mock => mock.ElementType).Returns(queryableBookData.ElementType);
            _bookMock.As<IQueryable<Book>>().Setup(mock => mock.Expression).Returns(queryableBookData.Expression);
            _bookMock.As<IQueryable<Book>>().Setup(mock => mock.Provider).Returns(queryableBookData.Provider);
            _bookMock.As<IQueryable<Book>>().Setup(mock => mock.GetEnumerator()).Returns(() => bookData.GetEnumerator());

            IQueryable<Tome> queryableTomeData = tomeData.AsQueryable();
            _tomeMock = new Mock<DbSet<Tome>>();
            _tomeMock.As<IQueryable<Tome>>().Setup(mock => mock.ElementType).Returns(queryableTomeData.ElementType);
            _tomeMock.As<IQueryable<Tome>>().Setup(mock => mock.Expression).Returns(queryableTomeData.Expression);
            _tomeMock.As<IQueryable<Tome>>().Setup(mock => mock.Provider).Returns(queryableTomeData.Provider);
            _tomeMock.As<IQueryable<Tome>>().Setup(mock => mock.GetEnumerator()).Returns(() => tomeData.GetEnumerator());

            IQueryable<Loan> queryableLoanData = loanData.AsQueryable();
            _loanMock = new Mock<DbSet<Loan>>();
            _loanMock.As<IQueryable<Loan>>().Setup(mock => mock.ElementType).Returns(() => queryableLoanData.ElementType);
            _loanMock.As<IQueryable<Loan>>().Setup(mock => mock.Expression).Returns(() => queryableLoanData.Expression);
            _loanMock.As<IQueryable<Loan>>().Setup(mock => mock.Provider).Returns(() => queryableLoanData.Provider);
            _loanMock.As<IQueryable<Loan>>().Setup(mock => mock.GetEnumerator()).Returns(() => loanData.GetEnumerator());

            _bookMock.Setup(mock => mock.Add(It.IsAny<Book>())).Callback<Book>(book =>
            {
                bookData.Add(book);
            });

            _tomeMock.Setup(mock => mock.Add(It.IsAny<Tome>())).Callback<Tome>(tome =>
            {
                tomeData.Add(tome);
            });

            _tomeMock.Setup(mock => mock.Remove(It.IsAny<Tome>())).Callback<Tome>(tome =>
            {
                tomeData.Remove(tome);
            });

            _loanMock.Setup(mock => mock.Add(It.IsAny<Loan>())).Callback<Loan>(loan =>
            {
                loanData.Add(loan);
            });

            _loanMock.Setup(mock => mock.Remove(It.IsAny<Loan>())).Callback<Loan>(loan =>
            {
                loanData.Remove(loan);
            });

            _entityMock = new Mock<LibraryContext>();
            _entityMock.Setup<DbSet<Book>>(entity => entity.Books).Returns(_bookMock.Object);
            _entityMock.Setup<DbSet<Tome>>(entity => entity.Tomes).Returns(_tomeMock.Object);
            _entityMock.Setup<DbSet<Loan>>(entity => entity.Loans).Returns(_loanMock.Object);
        }

        [Fact]
        public void GetBooksTest()
        {
            var controller = new BooksController(_entityMock.Object);
            var result = controller.GetBooks();

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<BookDTO>>(objectResult.Value);
            Assert.Equal(_bookDTOs, model);
        }

        [Fact]
        public void GetBookTest()
        {
            int bookId = 1;

            var controller = new BooksController(_entityMock.Object);
            var result = controller.GetBook(bookId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<BookDTO>(objectResult.Value);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void GetBookTest2()
        {
            int bookId = 2;

            var controller = new BooksController(_entityMock.Object);
            var result = controller.GetBook(bookId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<BookDTO>(objectResult.Value);
            Assert.Equal(2, model.Id);
        }

        [Fact]
        public void CreateBookTest()
        {
            //var count = _bookDTOs.Count();
            var newBook = new BookDTO
            {
                //Id = _bookDTOs.Count() + 1,
                Title = "TestTitle1",
                Author = "TestAuthor1",
                Year = 2001,
                ISBN = "12345678910",
                NumberOfLoans = 1
            };

            var controller = new BooksController(_entityMock.Object);
            var result = controller.PostBook(newBook);

            var objectResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<BookDTO>(objectResult.Value);
            Assert.Equal(_bookDTOs.Count() + 1, _entityMock.Object.Books.Count());
            //Assert.Equal(count, _entityMock.Object.Books.Count());
            Assert.Equal(newBook, model);
        }

        [Fact]
        public void GetTomesTest()
        {
            var controller = new TomesController(_entityMock.Object);
            var result = controller.GetTomes();

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<TomeDTO>>(objectResult.Value);
            Assert.Equal(_tomeDTOs, model);
        }

        [Fact]
        public void GetTomeTest()
        {
            int tomeId = 1;

            var controller = new TomesController(_entityMock.Object);
            var result = controller.GetTome(tomeId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<TomeDTO>(objectResult.Value);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void GetTomeTest2()
        {
            int tomeId = 2;

            var controller = new TomesController(_entityMock.Object);
            var result = controller.GetTome(tomeId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<TomeDTO>(objectResult.Value);
            Assert.Equal(2, model.Id);
            Assert.Equal(_tomeDTOs[1], model);
        }

        //a 2-es azonosítójú könyvhöz tartozó kötetek
        [Fact]
        public void GetTomesForBookTest()
        {
            int bookId = 2;
            var controller = new TomesController(_entityMock.Object);
            var result = controller.GetTomesForBook(bookId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<TomeDTO>>(objectResult.Value);
            Assert.Equal(_tomeDTOs.Where(l => l.BookId == 2), model);
        }

        [Fact]
        public void GetTomesForBookTest2()
        {
            int bookId = 1;
            var controller = new TomesController(_entityMock.Object);
            var result = controller.GetTomesForBook(bookId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<TomeDTO>>(objectResult.Value);
            Assert.Equal(_tomeDTOs.Where(l => l.BookId == 1), model);
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

            var controller = new TomesController(_entityMock.Object);
            var result = controller.PostTome(newTome);

            var objectResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<TomeDTO>(objectResult.Value);
            Assert.Equal(_tomeDTOs.Count + 1, _entityMock.Object.Tomes.Count());
            Assert.Equal(newTome, model);
        }

        [Fact]
        public void DeleteTomeTest()
        {
            var controller = new TomesController(_entityMock.Object);
            int deletedId = _entityMock.Object.Tomes.First().Id;
            var result = controller.DeleteTome(deletedId);

            Assert.IsType<OkResult>(result);
            Assert.Equal(_tomeDTOs.Count - 1, _entityMock.Object.Tomes.Count());
            Assert.DoesNotContain(deletedId, _entityMock.Object.Tomes.Select(b => b.Id));
        }

        [Fact]
        public void GetLoansTest()
        {
            var controller = new LoansController(_entityMock.Object);
            var result = controller.GetLoans();

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<LoanDTO>>(objectResult.Value);

            var loanToDelete = _loanDTOs.Where(l => l.LastDay < DateTime.Now).FirstOrDefault(); //egyet töröl a controller is
            _loanDTOs.Remove(loanToDelete);

            Assert.Equal(_loanDTOs.Count(), model.Count());
        }

        //eredetileg 3 megfelelő kölcsönzés van a tesztadatbázisban, de ebből egyet a dátum miatt kitörlünk, így marad kettő megfelelő
        [Fact]
        public void GetLoansForBookTest()
        {
            int bookId = 2; // ehhez van egy kötet és arra van két kölcsönzés
            var controller = new LoansController(_entityMock.Object);
            var result = controller.GetLoansForBook(bookId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<LoanDTO>>(objectResult.Value);
            Assert.Equal(_loanDTOs.Where(l => l.Tome.BookId == 2).Count() - 1, model.Count());
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void GetLoansForBookTest2()
        {
            int bookId = 1;
            var controller = new LoansController(_entityMock.Object);
            var result = controller.GetLoansForBook(bookId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<LoanDTO>>(objectResult.Value);
            Assert.Equal(_loanDTOs.Where(l => l.Tome.BookId == 1), model); //1 ilyen kölcsönzés van, és itt nem kell dátum miatt törölni
        }

        [Fact]
        public void GetLoansForTomeTest()
        {
            int tomeId = 2;
            var controller = new LoansController(_entityMock.Object);
            var result = controller.GetLoansForTome(tomeId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<LoanDTO>>(objectResult.Value);
            Assert.Equal(_loanDTOs.Where(l => l.TomeId == 2 && l.LastDay >= DateTime.Now).Select(l => l.Id), model.Select(l => l.Id));
        }

        [Fact]
        public void GetLoansForTomeTest2()
        {
            int tomeId = 3;
            var controller = new LoansController(_entityMock.Object);
            var result = controller.GetLoansForTome(tomeId);

            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<LoanDTO>>(objectResult.Value);
            foreach(var loan in _loanDTOs)
            {
                Debug.WriteLine(loan.LastDay);
            }
            
            Assert.Equal(_loanDTOs.Where(l => l.TomeId == 3 && l.LastDay >= DateTime.Now).Select(l => l.Id).ToList(), model.Select(l => l.Id).ToList());
        }

        [Fact]
        public void EditLoanTest()
        {
            var editedLoan = new LoanDTO
            {
                Id = 2,
                TomeId = _tomeDTOs.First().Id,
                Tome = _tomeDTOs.First(),
                IsActive = true,
                FirstDay = new DateTime(2020, 06, 14),
                LastDay = new DateTime(2020, 07, 01)
            };

            var controller = new LoansController(_entityMock.Object);
            var result = controller.PutLoan(editedLoan);

            Assert.IsType<OkResult>(result);
            Assert.Equal(_tomeDTOs.Count, _entityMock.Object.Loans.Count());
        }
    }
}
