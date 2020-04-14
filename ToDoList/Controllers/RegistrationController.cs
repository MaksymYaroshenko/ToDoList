using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Database;
using ToDoList.Database.Entities;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class RegistrationController : Controller
    {
        DatabaseContext db;

        public RegistrationController()
        {
            db = new DatabaseContext();
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationModel registrationModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var email in db.Users)
                    {
                        if (email.Email == registrationModel.Email)
                        {
                            ViewBag.Message = "The user with such Email exist";
                            return View(registrationModel);
                        }
                    }
                    User user = new User
                    {
                        Email = registrationModel.Email,
                        Password = registrationModel.Password,
                        Login = registrationModel.Login
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                    HttpContext.Session.SetString("Login", registrationModel.Login);
                    return RedirectToAction("Index", "Home");
                }
                return View(registrationModel);
            }
            catch
            {
                return RedirectToAction("Error404", "Home");
            }
        }
    }
}