using System.ComponentModel.DataAnnotations;

namespace AudioShmaudio.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name ="Логин")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        public string ConfirmPassword { get; set; }
    }
}
