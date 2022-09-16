namespace Blog.Models.DB
{
    public class ArticleLike
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
    }
}