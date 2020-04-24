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
                    foreach (var user in db.Users)
                    {
                        if (user.Email == signInModel.Email && user.Password == signInModel.Password)
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