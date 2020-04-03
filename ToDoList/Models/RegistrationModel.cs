using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class RegistrationModel
    {
        [BindNever]
        public int Id { get; set; }

        [Display(Name = "Enter your email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(30)]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Enter your password")]
        [DataType(DataType.Password)]
        [StringLength(30)]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Enter your login")]
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Login { get; set; }
    }
}
