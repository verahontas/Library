using Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Service.Test
{
    public static class TestDbInitializer
    {
        /*
        public static void Initialize(LibraryContext context)
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

            foreach (Book book in bookData)
            {
                context.Books.Add(book);
            }

            foreach(Tome tome in tomeData)
            {
                context.Tomes.Add(tome);
            }

            foreach (Loan loan in loanData)
            {
                context.Loans.Add(loan);
            }

            context.SaveChanges();
        }*/
    }
}
