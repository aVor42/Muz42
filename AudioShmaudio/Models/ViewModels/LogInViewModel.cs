using System.ComponentModel.DataAnnotations;

namespace AudioShmaudio.Models
{
    public class LogInViewModel
    {
        [Required]
        [Display(Name= "Логин", Prompt = "Логин")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Пароль", Prompt = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
