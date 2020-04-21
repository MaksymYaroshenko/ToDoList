using System;

namespace ToDoList.Database.Entities
{
    public class Task
    {
        public int ID { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public bool IsImportant { get; set; }

        public bool IsDone { get; set; }

        public int UserId { get; set; }
    }
}
