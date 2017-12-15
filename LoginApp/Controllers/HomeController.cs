using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LoginApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly LoginModels db;
        public HomeController(LoginModels context) { db = context; }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(LoginUser u)
        {


            if (u.Email != null) {
                var query = from user in db.Users where  user.Email == u.Email & user.Password == u.Password select user;

                if (query.Count() == 1)
                {
                    Users user = query.First<Users>();
                    var claims = new List<Claim> {
                        new Claim(ClaimTypes.Email, u.Email, ClaimValueTypes.String)
                    };

                    var userIdentity = new ClaimsIdentity(claims, "Login");
                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                   await HttpContext.SignInAsync(
                     CookieAuthenticationDefaults.AuthenticationScheme,
                     userPrincipal,
                     new AuthenticationProperties
                     {
                         IsPersistent = true
                     });

                    ViewData["Err_Msg"] = "Correct Login. ";
                }
                else
                {
                    ViewData["Err_Msg"] = "Invalid Login.";
                }
            }

            return View();
        }

        [Authorize]
        public IActionResult Priv()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("/Home/Index/");
        }
    
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
