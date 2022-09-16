using System.ComponentModel.DataAnnotations;

namespace Blog.Web.ViewModels
{
    public class CommentEditViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Content { get; set; }
    }
}