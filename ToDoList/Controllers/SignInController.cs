using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Database;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class SignInController : Controller
    {
        private DatabaseContext db;

        public SignInController()
        {
            db = new DatabaseContext();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(SignInModel signInModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PasswordHasher passwordHasher = new PasswordHasher();
                    foreach (var user in db.Users)
                    {
                        var result = passwordHasher.VerifyHashedPassword(user.Password, signInModel.Password);
                        if (user.Email == signInModel.Email && result == PasswordVerificationResult.Success)
                        {
                            HttpContext.Session.SetString("Login", user.Login);
                            TempData["currentUser"] = user.Id;
                            return RedirectToAction("Index", "Home");
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

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn");
        }
    }
}