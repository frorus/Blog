using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class CommentViewModel
    {
        public Guid ArticleId { get; set; }
        [Required]
        public string? Content { get; set; }
    }
}
