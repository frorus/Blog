using System.ComponentModel.DataAnnotations;

namespace Blog.Models.DB
{
    public class Tag
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
