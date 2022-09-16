using Blog.Core.Models;

namespace Blog.Web.ViewModels
{
    public class UserProfileViewModel
    {
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public DateTime JoinedDate { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Education { get; set; }
        public string Work { get; set; }
        public string Learning { get; set; }
        public string Skills { get; set; }
        public int Comments { get; set; }
        public List<Article> Articles { get; set; }
    }
}