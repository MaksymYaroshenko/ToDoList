using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers
{
    public class SignInController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
    }
}