using Blog.Data.Repository;
using Blog.Data.UnitOfWork;
using Blog.Models.DB;
using Microsoft.AspNetCore.Mvc;
namespace Blog.Controllers
{
    public class SearchController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> SearchPosts(string search)
        {
            ViewData["Search"] = search;

            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            IEnumerable<Article> articles = await repository.GetAllArticles();

            var articleList = articles.Where(x => x.Title.ToLower().Contains(search.ToLower()));

            return View(articleList);
        }
    }
}
