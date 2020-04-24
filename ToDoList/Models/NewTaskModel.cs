using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class NewTaskModel
    {
        [Display(Name = "Enter your task")]
        [StringLength(150, MinimumLength = 3)]
        [Required]
        public string TaskText { get; set; }

        public int UserId { get; set; }
    }
}
