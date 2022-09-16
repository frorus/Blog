﻿using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        public Guid ArticleId { get; set; }

        [Required]
        [StringLength(250)]
        public string Content { get; set; }
    }
}