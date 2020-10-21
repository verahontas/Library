using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class AccountController : Controller
    {
        private readonly SignInManager<Guest> _signInManager;

        public AccountController(SignInManager<Guest> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet("login/{userName}/{userPassword}")]
        public async Task<IActionResult> Login(String userName, String userPassword)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(userName, userPassword, false, false);
                if (!result.Succeeded)
                    return Forbid();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}