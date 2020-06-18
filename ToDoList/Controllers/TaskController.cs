using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ToDoList.Database;
using ToDoList.Database.Entities;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly DatabaseContext db;

        public TaskController()
        {
            db = new DatabaseContext();
        }

        public IActionResult Index()
        {
            try
            {
                var userName = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value;
                ViewBag.Login = userName;
                ViewBag.CurrentUser = userId;
                IEnumerable<Task> allTasks = db.Tasks.Where(i => i.UserId == Convert.ToInt32(userId)).OrderBy(i => i.IsDone).ThenByDescending(i => i.Date);
                TaskListModel model = new TaskListModel
                {
                    Tasks = allTasks
                };
                TaskModel taskModel = new TaskModel
                {
                    TaskListModel = model
                };
                return View(taskModel);
            }
            catch
            {
                return RedirectToAction("Error404");
            }
        }

        [Route("Task/Index/{category}")]
        public IActionResult Index(string category)
        {
            try
            {
                if (String.IsNullOrEmpty(category))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var userName = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                    var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value;
                    ViewBag.Login = userName;
                    IEnumerable<Task> tasks = null;
                    IEnumerable<Task> allTasks = db.Tasks.Where(i => i.UserId == Convert.ToInt32(userId)).OrderBy(i => i.IsDone).ThenByDescending(i => i.Date);
                    if (string.Equals("important", category, StringComparison.OrdinalIgnoreCase))
                    {
                        tasks = allTasks.Where(i => i.IsImportant == true);
                    }

                    if (string.Equals("myday", category, StringComparison.OrdinalIgnoreCase))
                    {
                        tasks = allTasks.Where(i => i.Date.Date == DateTime.Today);
                    }

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

        [HttpPut]
        public IActionResult MakeImportant(Task task)
        {
            try
            {
                var taskForCompleting = db.Tasks.Where(i => i.ID == task.ID).ToList();
                if (taskForCompleting[0].IsImportant)
                    taskForCompleting.ForEach(i => i.IsImportant = false);
                else
                    taskForCompleting.ForEach(i => i.IsImportant = true);
                db.Tasks.Update(taskForCompleting.First());
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error404");
            }
        }
    }
}