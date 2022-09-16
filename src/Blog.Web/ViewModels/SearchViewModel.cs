using Blog.Core.Models;

namespace Blog.Web.ViewModels
{
    public class SearchViewModel
    {
        public List<Article> Articles { get; set; }
        public List<Tag> Tags { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}