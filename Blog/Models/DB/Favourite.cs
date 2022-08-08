using System.ComponentModel.DataAnnotations;

namespace Blog.Models.DB
{
    public class Favourite
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public List<Article> FavouriteArticles { get; set; } = new List<Article>();
    }
}
