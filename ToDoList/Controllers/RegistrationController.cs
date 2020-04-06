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
            if (ModelState.IsValid)
            {
                bool exist = false;
                foreach (var email in db.Users)
                {
                    if (email.Email == registrationModel.Email)
                    {
                        exist = true;
                        ViewBag.Message = "The user with such Email exist";
                        return View(registrationModel);
                    }
                }
                if (!exist)
                {
                    User user = new User();
                    user.Email = registrationModel.Email;
                    user.Password = registrationModel.Password;
                    user.Login = registrationModel.Login;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(registrationModel);
        }
    }
}