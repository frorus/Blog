using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class ArticleViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
