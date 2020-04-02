using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }
    }
}