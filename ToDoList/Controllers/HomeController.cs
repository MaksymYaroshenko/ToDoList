using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            try
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
                    IEnumerable<Task> tasks = db.Tasks.Where(i => i.UserId == Convert.ToInt32(tempData[1])).OrderBy(i => i.IsDone).ThenByDescending(i => i.Date);
                    TaskListModel model = new TaskListModel
                    {
                        Tasks = tasks
                    };
                    TaskModel taskModel = new TaskModel
                    {
                        TaskListModel = model
                    };
                    return View(taskModel);
                }
            }
            catch
            {
                return RedirectToAction("Error404");
            }
        }

        [HttpPost]
        public IActionResult AddNewTask(NewTaskModel newTaskModel)
        {
            try
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
            catch
            {
                return RedirectToAction("Error404");
            }
        }

        [HttpDelete]
        public IActionResult DeleteTask(Task task)
        {
            try
            {
                var taskForDeleting = db.Tasks.Where(i => i.ID == task.ID).FirstOrDefault();
                db.Tasks.Remove(taskForDeleting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error404");
            }
        }

        [HttpPut]
        public IActionResult CompleteTask(Task task)
        {
            try
            {
                var taskForCompleting = db.Tasks.Where(i => i.ID == task.ID).ToList();
                if (taskForCompleting[0].IsDone)
                    taskForCompleting.ForEach(i => i.IsDone = false);
                else
                    taskForCompleting.ForEach(i => i.IsDone = true);
                db.Tasks.Update(taskForCompleting.First());
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error404");
            }
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
