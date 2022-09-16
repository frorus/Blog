using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class ArticleViewModel
    {
        [Required(ErrorMessage = "Поле заголовка не должно быть пустым")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "Заголовок должен быть от 5 до 150 символов")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Поле текста статьи не должно быть пустым")]
        [MinLength(50, ErrorMessage = "Текст статьи должен быть от 50 символов")]
        public string Text { get; set; }

        public List<SelectListItem> Tags { get; set; }
    }
}