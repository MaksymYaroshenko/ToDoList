using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using ToDoList.Database;
using ToDoList.Database.Entities;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DatabaseContext db;

        public HomeController(ILogger<HomeController> logger)
        {
            db = new DatabaseContext();
            _logger = logger;
        }

        public IActionResult Index()
        {
            var login = HttpContext.Session.GetString("Login");
            if (string.IsNullOrEmpty(login))
            {
                return RedirectToAction("SignIn", "SignIn");
            }
            else
            {
                if (TempData["currentUser"] != null)
                {
                    ViewBag.CurrentUser = (int)TempData["currentUser"];
                }
                ViewBag.Login = login;
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddNewTask(NewTaskModel newTaskModel)
        {
            Task task = new Task()
            {
                Date = DateTime.Now,
                Text = newTaskModel.TaskText,
                UserId = newTaskModel.UserId
            };
            db.Tasks.Add(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Error404()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
