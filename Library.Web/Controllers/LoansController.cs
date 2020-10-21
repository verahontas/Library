using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Library.Model;

namespace Library.Controllers
{
    public class LoansController : BaseController
    {
        private readonly UserManager<Guest> _userManager;

        public LoansController(ILoanService loanService, ApplicationState applicationState,
            UserManager<Guest> userManager)
            : base(loanService, applicationState)
        {
            _userManager = userManager;
        }

        //kölcsönzés leadása
        [HttpGet]
        public async Task<IActionResult> Index(Int32? tomeId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            LoanViewModel loan = _loanService.NewLoan(tomeId);

            if (loan == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (User.Identity.IsAuthenticated)
            {
                Guest guest = await _userManager.FindByNameAsync(User.Identity.Name);

                if (guest != null)
                {
                    loan.GuestEmail = guest.Email;
                    loan.GuestName = guest.Name;
                    loan.GuestPhoneNumber = guest.PhoneNumber;
                }
            }

            return View("Index", loan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Int32? tomeId, LoanViewModel loan)
        {
            if (tomeId == null || loan == null)
                return RedirectToAction("Index", "Home");

            loan.Tome = _loanService.GetTome(tomeId);

            if (loan.Tome == null)
                return RedirectToAction("Index", "Home");

            switch (_loanService.ValidateLoan(loan.LoanFirstDay, loan.LoanLastDay, tomeId.Value))
            {
                case LoanDateError.StartInvalid:
                    ModelState.AddModelError("LoanFirstDay", "A kezdés dátuma nem megfelelő!");
                    break;
                case LoanDateError.EndInvalid:
                    ModelState.AddModelError("LoanLastDay", "A megadott kölcsönzési idő érvénytelen (a foglalás vége korábban van, mint a kezdete)!");
                    break;
                case LoanDateError.Conflicting:
                    ModelState.AddModelError("LoanFirstDay", "A megadott időpontban a könyvre már van előjegyzés!");
                    break;
                case LoanDateError.LengthInvalid:
                    ModelState.AddModelError("LoanLastDay", "A kező és befejező nap egy napra esik!");
                    break;
            }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in allErrors)
            {
                Debug.WriteLine(error);
            }

            if (!ModelState.IsValid)
                return View("Index", loan);

            Guest guest;
            
            if (User.Identity.IsAuthenticated)
            {
                guest = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            else
            {
                guest = new Guest
                {
                    UserName = "user" + Guid.NewGuid(),
                    Email = loan.GuestEmail,
                    Name = loan.GuestName,
                    PhoneNumber = loan.GuestPhoneNumber
                };
                var result = await _userManager.CreateAsync(guest);
                await _userManager.AddToRoleAsync(guest, "Visitor");
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "A kölcsönzés sikertelen!");
                    return View("Index", loan);
                }
            }

            if (!await _loanService.SaveLoanAsync(tomeId, guest.UserName, loan))
            {
                ModelState.AddModelError("", "A kölcsönzés sikertelen!");
                return View("Index", loan);
            }

            ViewBag.Message = "A kölcsönzést sikeresen rögzítettük!";
            return View("Result", loan);
        }

        //[Authorize(Roles="Librarian")]
        public void ChangeStatus(int? loanId)
        {
            bool succeeded = _loanService.ChangeLoanStatus(loanId);
            if (succeeded)
                ViewBag.Message = "A kölcsönzés státusza sikeresen megváltozott!";
            else
                ViewBag.Message = "Erre a kötetre már van aktív kölcsönzés, egyszerre csak egy kölcsönzés lehet aktív!";
        }
    }
}
