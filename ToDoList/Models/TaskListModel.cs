using System.Collections.Generic;
using ToDoList.Database.Entities;

namespace ToDoList.Models
{
    public class TaskListModel
    {
        public IEnumerable<Task> Tasks { get; set; }
    }
}
