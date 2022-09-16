using System.ComponentModel.DataAnnotations;

namespace Blog.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Почта введена некорректно")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Поле должно иметь минимум {2} символов", MinimumLength = 5)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}