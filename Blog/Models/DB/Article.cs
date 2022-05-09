namespace Blog.Models.DB
{
    public class Article
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
