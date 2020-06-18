using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Database;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class SignInController : Controller
    {
        private readonly DatabaseContext db;

        public SignInController()
        {
            db = new DatabaseContext();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInModel signInModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = db.Users.FirstOrDefault(e => e.Email == signInModel.Email);
                    if (user != null)
                    {
                        PasswordHasher passwordHasher = new PasswordHasher();
                        var result = passwordHasher.VerifyHashedPassword(user.Password, signInModel.Password);
                        if (result == PasswordVerificationResult.Success)
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.Login), // User Name
                                new Claim(ClaimTypes.PrimarySid, user.Id.ToString()) // User ID
                            };
                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);
                            var props = new AuthenticationProperties();
                            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();
                            return RedirectToAction("Index", "Task");
                        }
                    }
                    ViewBag.Message = "We cannot find such account. Please check Email and password";
                    return View(signInModel);
                }
                return View(signInModel);
            }
            catch
            {
                return RedirectToAction("Error404", "Home");
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}