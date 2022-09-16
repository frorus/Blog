using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class TagViewModel
    {
        [Required(ErrorMessage = "Поле заголовка не должно быть пустым")]
        [StringLength(100, ErrorMessage = "Поле должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
    }
}