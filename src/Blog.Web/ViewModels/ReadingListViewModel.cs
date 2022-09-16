using Blog.Core.Models;

namespace Blog.Web.ViewModels
{
    public class ReadingListViewModel
    {
        public Guid UserId { get; set; }
        public List<Article> Articles { get; set; }
        public List<string> Tags { get; set; }
    }
}