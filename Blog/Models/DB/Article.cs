namespace Blog.Models.DB
{
    public class Article
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Title { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<ArticleLike> ArticleLikes { get; set; } = new List<ArticleLike>();
        public List<Favourite> Favourites { get; set; } = new List<Favourite>();
    }
}