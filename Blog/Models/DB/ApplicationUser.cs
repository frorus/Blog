using Microsoft.AspNetCore.Identity;

namespace Blog.Models.DB
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Website { get; set; }
        public string Location { get; set; }
        public string Bio { get; set; }
        public string Learning { get; set; }
        public string Skills { get; set; }
        public string Work { get; set; }
        public string Education { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.Now;
        public List<Article> Articles { get; set; } = new List<Article>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
