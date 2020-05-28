using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
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
                    var user = db.Users.FirstOrDefault(e => e.Email == signInModel.Email);
                    if (user != null)
                    {
                        PasswordHasher passwordHasher = new PasswordHasher();
                        var result = passwordHasher.VerifyHashedPassword(user.Password, signInModel.Password);
                        if (result == PasswordVerificationResult.Success)
                        {
                            HttpContext.Session.SetString("Login", user.Login);
                            TempData["currentUser"] = user.Id;
                            WriteToTemp(user.Login, user.Id);
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

        private void WriteToTemp(string userLogin, int userId)
        {
            string tempPath = Path.GetTempPath();
            using StreamWriter tempFile = new StreamWriter(Path.Combine(tempPath, "ToDoListAppTemp.txt"), true);
            tempFile.WriteLine(userLogin);
            tempFile.WriteLine(userId);
        }
    }
}