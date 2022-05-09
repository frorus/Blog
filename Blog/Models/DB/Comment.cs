namespace Blog.Models.DB
{
    public class Comment
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; } = null!;
        public int UserId { get; set; }
        public int ArticleId { get; set; }
        public User User { get; set; } = null!;
        public Article Article { get; set; } = null!;
    }
}
