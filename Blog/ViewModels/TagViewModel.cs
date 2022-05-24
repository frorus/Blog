using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class TagViewModel
    {
        [Required]
        public string? Title { get; set; }
    }
}
