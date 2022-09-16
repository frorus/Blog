using Blog.Core.Models;

namespace Blog.Web.ViewModels
{
    public class ArticleDetailsViewModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<Article> UserArticles { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Tag> Tags { get; set; }
        public List<ArticleLike> ArticleLikes { get; set; }
        public List<Favourite> Favourites { get; set; }
    }
}