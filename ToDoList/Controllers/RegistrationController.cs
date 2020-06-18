using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ToDoList.Database;
using ToDoList.Database.Entities;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class RegistrationController : Controller
    {
        readonly DatabaseContext db;

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
                    User user = db.Users.FirstOrDefault(e => e.Email == registrationModel.Email);
                    if (user == null)
                    {
                        PasswordHasher passwordHash = new PasswordHasher();
                        user = new User
                        {
                            Email = registrationModel.Email,
                            Password = passwordHash.HashPassword(registrationModel.Password),
                            Login = registrationModel.Login
                        };
                        db.Users.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Task");
                    }
                    else
                    {
                        ViewBag.Message = "The user with such Email exist";
                        return View(registrationModel);
                    }
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