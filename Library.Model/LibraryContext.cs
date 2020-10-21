using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Library.Model
{
    public class LibraryContext : IdentityDbContext<Guest, IdentityRole<int>, int>
    {
        public LibraryContext() { }

        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Guest>().ToTable("Guests");
        }

        public virtual DbSet<Book> Books { get; set; }

        public virtual DbSet<BookImage> BookImages { get; set; }

        public virtual DbSet<Loan> Loans { get; set; }

        public virtual DbSet<Tome> Tomes { get; set; }
    }
}
