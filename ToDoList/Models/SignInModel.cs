using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class SignInModel
    {
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
    }
}
