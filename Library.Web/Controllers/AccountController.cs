using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Model;
using Library.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<Guest> _userManager;
        
        private readonly SignInManager<Guest> _signInManager;

        public AccountController(ILoanService loanService, ApplicationState applicationState,
            UserManager<Guest> userManager, SignInManager<Guest> signInManager)
            : base(loanService, applicationState)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
                return View("Login", user);

            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.UserPassword, user.RememberLogin, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Hibás felhasználónév, vagy jelszó.");
                return View("Login", user);
            }
    
            _applicationState.UserCount++;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel user)
        {
            if (!ModelState.IsValid)
                return View("Register", user);

            Guest guest = new Guest
            {
                UserName = user.UserName,
                Email = user.GuestEmail,
                Name = user.GuestName,
                PhoneNumber = user.GuestPhoneNumber
            };
            var result = await _userManager.CreateAsync(guest, user.UserPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View("Register", user);
            }

            //csak az admin-admin a könyvtáros, mindenki más látogató
            //könyvtárosokat kódból lehet hozzáadni
            //await _userManager.AddToRoleAsync(guest, "Visitor");

            await _signInManager.SignInAsync(guest, false);
            _applicationState.UserCount++;
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            _applicationState.UserCount--;
            return RedirectToAction("Index", "Home");
        }
    }
}