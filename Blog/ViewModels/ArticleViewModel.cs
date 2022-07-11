using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class ArticleViewModel
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Text { get; set; }

        public List<SelectListItem> Tags { get; set; }
    }
}
