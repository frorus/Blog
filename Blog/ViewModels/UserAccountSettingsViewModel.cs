using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class UserAccountSettingsViewModel
    {
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Поле должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
        public string PasswordCurrent { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Поле должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
        public string PasswordNew { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Compare("PasswordNew", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}