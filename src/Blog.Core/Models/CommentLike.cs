﻿namespace Blog.Core.Models
{
    public class CommentLike
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}