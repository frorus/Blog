using System.ComponentModel.DataAnnotations;

namespace Blog.Models.DB
{
    public class Tag
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле заголовка не должно быть пустым")]
        [StringLength(100, ErrorMessage = "Поле должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public List<Article> Articles { get; set; } = new List<Article>();
    }
}