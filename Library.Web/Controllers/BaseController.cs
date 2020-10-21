using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Web;

namespace Library.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILoanService _loanService;
        protected readonly ApplicationState _applicationState;

        public BaseController(ILoanService loanService, ApplicationState applicationState)
        {
            _applicationState = applicationState;
            _loanService = loanService;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            ViewBag.UserCount = _applicationState.UserCount;
            ViewBag.CurrentGuestName = String.IsNullOrEmpty(User.Identity.Name) ? null : User.Identity.Name;
            //ViewBag.CurrentGuestName = String.IsNullOrEmpty(HttpContext.User.Identity.Name) ? null : User.Identity.Name;
            ViewBag.Loans = _loanService.Loans;
            ViewBag.Books = _loanService.Books;
        }
    }
}