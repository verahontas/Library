using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Web.Models;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Library.Model;

namespace Library.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ILoanService loanService, ApplicationState applicationState)
            : base(loanService, applicationState)
        {}

        [HttpGet]
        public IActionResult Index(string currentFilter, string searchString, int pageNumber = 1, string sortOrder = "")
        {
            try
            {
                var books = _loanService.Books;
                var loans = _loanService.Loans;

                if (searchString != null)
                {
                    pageNumber = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                //ha a keresőstring nem üres, először szűrjük az összes könyvet
                if (!String.IsNullOrEmpty(searchString))
                {
                    books = books.Where(l => l.Title.ToUpper().Contains(searchString.ToUpper()) || l.Author.ToUpper().Contains(searchString.ToUpper()));
                }

                ViewData["CurrentSort"] = sortOrder;
                ViewData["PopularitySortParam"] = string.IsNullOrEmpty(sortOrder) ? "popularity_desc" : "";
                ViewData["TitleSortParam"] = sortOrder == "title_asc" ? "title_desc" : "title_asc";
                ViewData["CurrentFilter"] = searchString;

                List<Book> result = new List<Book>();

                switch (sortOrder)
                {
                    case "title_desc":
                        result = books.OrderByDescending(l => l.Title).ToList();
                        break;
                    case "title_asc":
                        result = books.OrderBy(l => l.Title).ToList();
                        break;
                    case "popularity_desc":
                        result = books.OrderBy(l => l.NumberOfLoans).ToList();
                        break;
                    default:
                        result = books.OrderByDescending(l => l.NumberOfLoans).ToList();
                        break;
                }

                int pageSize = 5;
                IQueryable<Book> res = result.AsQueryable();
                //return View(await PaginatedList<Book>.CreateAsync(res.AsNoTracking(), pageNumber ?? 1, pageSize));
                return View(PaginatedList<Book>.CreateAsync(res.AsNoTracking(), pageNumber, pageSize));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        public IActionResult Details(int? bookId)
        {
            if (bookId == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var book = _loanService.GetBook(bookId);

            if (bookId == null)
            {
                return NotFound();
            }

            ViewBag.Title = "A könyv részletei:";
            ViewBag.Images = _loanService.GetBookImageIds(bookId).ToList();

            return View("Details", book);
        }

        public FileResult ImageForBook(Int32? bookId)
        {
            if (bookId == null)
                return File("~/images/NoImage.png", "image/png");

            Byte[] imageContent = _loanService.GetBookImage(bookId);
            if (imageContent == null)
                return File("~/images/NoImage.png", "image/png");

            return File(imageContent, "image/png");
        }

        public FileResult Image(Int32? imageId, Boolean large = false)
        {
            if (imageId == null)
                return File("~/images/NoImage.png", "image/png");

            byte[] imageContent = _loanService.GetBookImage(imageId, large);
            if (imageContent == null)
                return File("~/images/NoImage.png", "image/png");

            return File(imageContent, "image/png");
        }

        //[Authorize(Roles ="Librarian")]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ISBN,Title,Author,Year")] Book book)
        {
            if (ModelState.IsValid)
            {
                _loanService.AddBook(book);
                ViewBag.Message = "Könyv sikeresen hozzáadva!";
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

    }
}
