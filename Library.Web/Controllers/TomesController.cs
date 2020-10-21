using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Library.Model;

namespace Library.Controllers
{
    //[Authorize(Roles = "Librarian")]
    public class TomesController : BaseController
    {
        public TomesController(ILoanService loanService, ApplicationState applicationState)
            : base(loanService, applicationState)
        { }
        
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_loanService.Books.Select(l => l.Id));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,BookId")] Tome tome)
        {
            var book = _loanService.Books.FirstOrDefault(l => l.Id == tome.BookId);
            tome.Book = book;

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in allErrors)
            {
                Debug.WriteLine(error);
            }

            if (ModelState.IsValid)
            {
                if (_loanService.AddTome(tome))
                {
                    return View("AddTomeDone", tome);
                }
            }

            ViewBag.Message = "Kötet létrehozása sikertelen!";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Delete(int? tomeId)
        {
            if (tomeId == null)
            {
                ViewBag.Message = "A törlendő kötet nem létezik.";
                return RedirectToAction("Index", "Home");
            }

            var tome = _loanService.Tomes.FirstOrDefault(l => l.Id == tomeId);
            if (tome == null)
            {
                ViewBag.Message = "A törlendő kötet nem létezik.";
                return RedirectToAction("Index", "Home");
            }

            return View(tome);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int tomeId)
        {
            var tome = _loanService.Tomes.FirstOrDefault(l => l.Id == tomeId);

            if (_loanService.RemoveTome(tomeId)) {
                ViewBag.Message = "A kötet sikeresen törölve!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Message = "A kötet törlése sikertelen!";
            return RedirectToAction(nameof(Index));
        }

        private bool TomeExists(int id)
        {
            return _loanService.Tomes.Any(e => e.Id == id);
        }
    }
}
