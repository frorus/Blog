using Blog.Models.DB;

namespace Blog.ViewModels
{
    public class SearchViewModel
    {
        public List<Article> Articles { get; set; }
        public List<Tag> Tags { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}