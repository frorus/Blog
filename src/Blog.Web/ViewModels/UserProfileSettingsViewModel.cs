using System.ComponentModel.DataAnnotations;

namespace Blog.Web.ViewModels
{
    public class UserProfileSettingsViewModel
    {
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Почта введена некорректно")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [StringLength(15)]
        public string Username { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImagePath { get; set; }

        [Url(ErrorMessage = "Сайт введен некорректно")]
        [StringLength(100)]
        public string Website { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        [StringLength(200)]
        public string Bio { get; set; }

        [StringLength(200)]
        public string Learning { get; set; }

        [StringLength(200)]
        public string Skills { get; set; }

        [StringLength(100)]
        public string Work { get; set; }

        [StringLength(100)]
        public string Education { get; set; }
    }
}