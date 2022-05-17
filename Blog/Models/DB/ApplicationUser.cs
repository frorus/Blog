using Microsoft.AspNetCore.Identity;

namespace Blog.Models.DB
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<Article> Articles { get; set; } = new List<Article>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
