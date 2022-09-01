using System.ComponentModel.DataAnnotations;

namespace Blog.Models.DB
{
    public class Article
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Text { get; set; } = null!;
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<ArticleLike> ArticleLikes { get; set; } = new List<ArticleLike>();
        public List<Favourite> Favourites { get; set; } = new List<Favourite>();
    }
}