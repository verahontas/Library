using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Model
{
    public static class DbInitializer
    {
        private static LibraryContext _context;
        private static UserManager<Guest> _userManager;
        private static RoleManager<IdentityRole<int>> _roleManager;
        public static void Initialize(IServiceProvider serviceProvider, string imageDirectory)
        {
            _context = serviceProvider.GetRequiredService<LibraryContext>();
            _userManager = serviceProvider.GetRequiredService<UserManager<Guest>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            //context.Database.EnsureCreated();
            _context.Database.Migrate();

            if (_context.Books.Any())
            {
                return;
            }

            SeedBooks();
            SeedTomes();
            SeedLoans();
            SeedBookImages(imageDirectory);
            SeedNumberOfLoans();

            if (!_context.Users.Any())
            {
                SeedUsers();
            }
        }

        private static void SeedUsers()
        {
            var adminUser = new Guest
            {
                UserName = "admin",
                Name = "Adminisztrátor",
                Email = "admin@example.com",
                PhoneNumber = "+36123456789"
            };
            var adminPassword = "Admin123";
            var adminRole = new IdentityRole<int>("administrator");

            var result1 = _userManager.CreateAsync(adminUser, adminPassword).Result;
            var result2 = _roleManager.CreateAsync(adminRole).Result;
            var result3 = _userManager.AddToRoleAsync(adminUser, adminRole.Name).Result;
        }

        private static void SeedBooks()
        {
            var defaultBooks = new Book[]
            {
                new Book
                {
                    Title = "Állattemető",
                    Author = "Stephen King",
                    Year = 1983,
                    ISBN = "12345678910",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "Tortúra",
                    Author = "Stephen King",
                    Year = 1987,
                    ISBN = "3281692417",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "Carrie",
                    Author = "Stephen King",
                    Year = 1974,
                    ISBN = "6629435135",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "Az",
                    Author = "Stephen King",
                    Year = 1986,
                    ISBN = "27651899635",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "A ragyogás",
                    Author = "Stephen King",
                    Year = 1977,
                    ISBN = "6669435135",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "Végítélet",
                    Author = "Stephen King",
                    Year = 1978,
                    ISBN = "6629491535",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "A kívülálló",
                    Author = "Stephen King",
                    Year = 2018,
                    ISBN = "886294325",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "The Institute",
                    Author = "Stephen King",
                    Year = 2019,
                    ISBN = "9157281532",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "Doctor Sleep",
                    Author = "Stephen King",
                    Year = 2013,
                    ISBN = "654793157",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "A halálsoron",
                    Author = "Stephen King",
                    Year = 1996,
                    ISBN = "219765348",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "If It Bleeds",
                    Author = "Stephen King",
                    Year = 2020,
                    ISBN = "3197654892",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "1922",
                    Author = "Stephen King",
                    Year = 2010,
                    ISBN = "9431567824",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "A köd",
                    Author = "Stephen King",
                    Year = 1980,
                    ISBN = "3469875613",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "Cujo",
                    Author = "Stephen King",
                    Year = 1981,
                    ISBN = "3164985673",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "A Búra alatt",
                    Author = "Stephen King",
                    Year = 2009,
                    ISBN = "3165894356"
                },
                new Book
                {
                    Title = "Borzalmak városa",
                    Author = "Stephen King",
                    Year = 1975,
                    ISBN = "946315827",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "A magas fűben",
                    Author = "Stephen King",
                    Year = 2012,
                    ISBN = "7645861232",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "Bilincsben",
                    Author = "Stephen King",
                    Year = 1992,
                    ISBN = "4568975314",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "Csontkollekció",
                    Author = "Stephen King",
                    Year = 1985,
                    ISBN = "6548315249",
                    NumberOfLoans = 0
                },
                new Book
                {
                    Title = "Christine",
                    Author = "Stephen King",
                    Year = 1983,
                    ISBN = "26978546315",
                    NumberOfLoans = 0
                }
            };

            foreach (var book in defaultBooks)
            {
                _context.Books.Add(book);
            }
            _context.SaveChanges();
        }

        private static int RandomNumberGenerator(int size)
        {
            Random random = new Random();
            return random.Next(1, size);
        }

        private static void SeedTomes()
        {
            int size = _context.Books.Count();
            var defaultTomes = new Tome[]
            {
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) },
                new Tome { BookId = RandomNumberGenerator(size) }
            };

            foreach (var tome in defaultTomes)
            {
                _context.Tomes.Add(tome);
            }
            _context.SaveChanges();
        }

        private static void SeedLoans()
        {
            //default 23 kötet van
            var defaultLoans = new Loan[]
            {
                new Loan
                {
                    TomeId = 1,
                    FirstDay = new DateTime(2020, 5, 28),
                    LastDay = new DateTime(2020, 6, 9)
                },
                new Loan
                {
                    TomeId = 1,
                    FirstDay = new DateTime(2020, 6, 14),
                    LastDay = new DateTime(2020, 6, 19)
                },
                new Loan
                {
                    TomeId = 2,
                    FirstDay = new DateTime(2020, 5, 1),
                    LastDay = new DateTime(2020, 5, 2)
                },
                new Loan
                {
                    TomeId = 3,
                    FirstDay = new DateTime(2020, 1, 4),
                    LastDay = new DateTime(2020, 3, 3)
                },
                new Loan
                {
                    TomeId = 4,
                    FirstDay = new DateTime(2020, 3, 1),
                    LastDay = new DateTime(2020, 3, 3)
                },
                new Loan
                {
                    TomeId = 5,
                    FirstDay = new DateTime(2020, 4, 11),
                    LastDay = new DateTime(2020, 4, 29)
                },
                new Loan
                {
                    TomeId = 5,
                    FirstDay = new DateTime(2020, 5, 02),
                    LastDay = new DateTime(2020, 5, 14)
                },
                new Loan
                {
                    TomeId = 6,
                    FirstDay = new DateTime(2020, 5, 4),
                    LastDay = new DateTime(2020, 5, 7)
                },
                new Loan
                {
                    TomeId = 6,
                    FirstDay = new DateTime(2020, 6, 1),
                    LastDay = new DateTime(2020, 7, 1)
                },
                new Loan
                {
                    TomeId = 5,
                    FirstDay = new DateTime(2020, 7, 2),
                    LastDay = new DateTime(2020, 7, 3)
                }
            };

            foreach (var loan in defaultLoans)
            {
                _context.Loans.Add(loan);
            }
            _context.SaveChanges();
        }

        private static void SeedBookImages(string imageDirectory)
        {

            if (Directory.Exists(imageDirectory))
            {
                var images = new List<BookImage>();

                var largePath = Path.Combine(imageDirectory, "allattemeto.jpg");
                var smallPath = Path.Combine(imageDirectory, "allattemeto.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "Állattemető").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "tortura.jpg");
                smallPath = Path.Combine(imageDirectory, "tortura_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "Tortúra").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "carrie.jpg");
                smallPath = Path.Combine(imageDirectory, "carrie_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "Carrie").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "az.jpg");
                smallPath = Path.Combine(imageDirectory, "az_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "Az").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "aragyogas.jpg");
                smallPath = Path.Combine(imageDirectory, "aragyogas_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "A ragyogás").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                System.Diagnostics.Debug.WriteLine(File.Exists(largePath));

                largePath = Path.Combine(imageDirectory, "vegitelet.jpg");
                smallPath = Path.Combine(imageDirectory, "vegitelet_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "Végítélet").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "akivulallo.jpg");
                smallPath = Path.Combine(imageDirectory, "akivulallo_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "A kívülálló").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "theinstitute.jpg");
                smallPath = Path.Combine(imageDirectory, "theinstitute_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "The Institute").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "doctorsleep.jpg");
                smallPath = Path.Combine(imageDirectory, "doctorsleep_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "Doctor Sleep").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "ahalalsoron.jpg");
                smallPath = Path.Combine(imageDirectory, "ahalalsoron_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "A halálsoron").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "ifitbleeds.jpg");
                smallPath = Path.Combine(imageDirectory, "ifitbleeds_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "If It Bleeds").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "1922.jpg");
                smallPath = Path.Combine(imageDirectory, "1922_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "1922").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "akod.jpg");
                smallPath = Path.Combine(imageDirectory, "akod_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "A köd").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "cujo.jpg");
                smallPath = Path.Combine(imageDirectory, "cujo_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "Cujo").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "aburaalatt.jpg");
                smallPath = Path.Combine(imageDirectory, "aburaalatt_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "A Búra alatt").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "borzalmakvarosa.jpg");
                smallPath = Path.Combine(imageDirectory, "borzalmakvarosa_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "Borzalmak városa").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "amagasfuben.jpg");
                smallPath = Path.Combine(imageDirectory, "amagasfuben_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "A magas fűben").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "bilincsben.jpg");
                smallPath = Path.Combine(imageDirectory, "bilincsben_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "Bilincsben").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "csontkollekcio.jpg");
                smallPath = Path.Combine(imageDirectory, "csontkollekcio_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "Csontkollekció").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "christine.jpg");
                smallPath = Path.Combine(imageDirectory, "christine_small.jpg");

                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new BookImage
                    {
                        BookId = _context.Books.FirstOrDefault(l => l.Title == "Christine").Id,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                foreach (var image in images)
                {
                    _context.BookImages.Add(image);
                }

                _context.SaveChanges();
            }
        }

        private static void SeedNumberOfLoans()
        {
            var loans = _context.Loans.Include(l => l.Tome);

            foreach (Loan loan in loans)
            {
                foreach (Book book in _context.Books)
                {
                    if (loan.Tome.BookId == book.Id)
                    {
                        book.NumberOfLoans += 1;
                    }
                }
            }

            _context.SaveChanges();
        }
    }
}
