using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class TagViewModel
    {
        [Required]
        public string? Title { get; set; }
        //[ValidateNever]
        public string? Description { get; set; }
    }
}
