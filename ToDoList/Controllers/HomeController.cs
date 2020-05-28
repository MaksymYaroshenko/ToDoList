using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            var tempData = GetDataFromTempFile();
            if (tempData == null)
            {
                return RedirectToAction("SignIn", "SignIn");
            }
            else
            {
                ViewBag.Login = tempData[0];
                ViewBag.CurrentUser = tempData[1];
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

        private string[] GetDataFromTempFile()
        {
            string tempPath = Path.GetTempPath();
            string tempFile = Path.Combine(tempPath, ConfigurationManager.AppSetting["TempFile:TempFile"]);
            if (System.IO.File.Exists(tempFile))
            {
                using StreamReader sr = new StreamReader(tempFile);
                var tempFileText = sr.ReadToEnd();
                var userLogin = tempFileText.Split('\r').First();
                var userId = tempFileText.Split('\n');
                return new string[] { userLogin, userId[1].Split('\r').First() };
            }
            else
                return null;
        }
    }
}
