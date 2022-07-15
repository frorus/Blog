using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class CommentEditViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string? Content { get; set; }
    }
}
