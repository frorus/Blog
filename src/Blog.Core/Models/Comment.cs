namespace Blog.Core.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Guid ArticleId { get; set; }
        public ApplicationUser User { get; set; }
        public Article Article { get; set; }
        public List<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
    }
}