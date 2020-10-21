using Library.Admin.Persistence;
using Library.Data;
using Library.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace Library.Service.Test
{
    
    /*public class LibraryIntegrationTest : IDisposable
    {
        
        public static IList<Book> BookData = new List<Book>
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
                Id = 1,
                Title = "TESTBOOKTITLE2",
                Author = "TESTBOOKAUTHOR2",
                Year = 2002,
                ISBN = "3649519767",
                NumberOfLoans = 1
            },
            new Book
            {
                Id = 2,
                Title = "TESTBOOKTITLE3",
                Author = "TESTBOOKAUTHOR3",
                Year = 2003,
                ISBN = "1976431867",
                NumberOfLoans = 0
            },
            new Book
            {
                Id = 2,
                Title = "TESTBOOKTITLE4",
                Author = "TESTBOOKAUTHOR4",
                Year = 2004,
                ISBN = "9761531867",
                NumberOfLoans = 1
            }
        };

        public static IList<Tome> TomeData = new List<Tome>
        {
            new Tome
            {
                Id = 1,
                Book = BookData[1],
                BookId = BookData[1].Id
            },
            new Tome
            {
                Id = 2,
                Book = BookData[0],
                BookId = BookData[0].Id
            },
            new Tome
            {
                Id = 3,
                Book = BookData[1],
                BookId = BookData[1].Id
            },
            new Tome
            {
                Id = 4,
                Book = BookData[2],
                BookId = BookData[2].Id
            }
        };

        public static IList<Loan> LoanData = new List<Loan>
        {
            new Loan
            {
                Id = 1,
                Tome = TomeData[2],
                TomeId = TomeData[2].Id,
                FirstDay = new DateTime(2020, 03, 04),
                LastDay = new DateTime(2020, 03, 05),
                IsActive = false
            },
            new Loan
            {
                Id = 2,
                Tome = TomeData[0],
                TomeId = TomeData[0].Id,
                FirstDay = new DateTime(2020, 05, 29),
                LastDay = new DateTime(2020, 06, 1),
                IsActive = false
            },
            new Loan
            {
                Id = 3,
                Tome = TomeData[3],
                TomeId = TomeData[3].Id,
                FirstDay = new DateTime(2020, 05, 19),
                LastDay = new DateTime(2020, 05, 29),
                IsActive = true
            },
            new Loan
            {
                Id = 4,
                Tome = TomeData[2],
                TomeId = TomeData[2].Id,
                FirstDay = new DateTime(2020, 03, 06),
                LastDay = new DateTime(2020, 03, 16),
                IsActive = false
            }
        };

        private readonly List<BookDTO> _bookDTOs;
        private readonly List<TomeDTO> _tomeDTOs;
        private readonly List<LoanDTO> _loanDTOs;
        private readonly ILibraryPersistence _persistence;

        private readonly IHost _server;
        private readonly HttpClient _client;

        public LibraryIntegrationTest()
        {
            _bookDTOs = BookData.Select(book => new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                NumberOfLoans = book.NumberOfLoans,
                Images = new List<ImageDTO>()
            }).ToList();

            _tomeDTOs = TomeData.Select(tome => new TomeDTO
            {
                Id = tome.Id,
                Book = _bookDTOs.FirstOrDefault(book => book.Id == tome.BookId),
                BookId = tome.BookId
            }).ToList();

            _loanDTOs = LoanData.Select(loan => new LoanDTO
            {
                Id = loan.Id,
                Tome = _tomeDTOs.FirstOrDefault(tome => tome.Id == loan.TomeId),
                TomeId = loan.TomeId,
                FirstDay = loan.FirstDay,
                LastDay = loan.LastDay,
                IsActive = loan.IsActive
            }).ToList();

            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost
                        .UseTestServer()
                        .UseStartup<TestStartup>()
                        .UseEnvironment("Development");
                });
            
            _server = hostBuilder.Start();

            _client = _server.GetTestClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _persistence = new LibraryServicePersistence(_client);
        }

        public void Dispose()
        {
            var dbContext = _server.Services.GetRequiredService<LibraryContext>();
            dbContext.Database.EnsureDeleted();
        }

        [Fact]
        public async void GetBooksTest()
        {
            IEnumerable<BookDTO> result = await _persistence.ReadBooksAsync();

            Assert.Equal(_bookDTOs, result);
        }

        [Fact]
        public async void GetTomesTest()
        {
            IEnumerable<TomeDTO> result = await _persistence.ReadTomesAsync();

            Assert.Equal(_tomeDTOs, result);
        }

        [Fact]
        public async void GetLoansTest()
        {
            IEnumerable<LoanDTO> result = await _persistence.ReadLoansAsync();

            Assert.Equal(_loanDTOs, result);
        }
    }*/
    
}
