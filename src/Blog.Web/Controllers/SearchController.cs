using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
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
            var articleList = await repository.GetAllArticles().Where(x => x.Title.ToLower().Contains(search.ToLower())).ToListAsync();

            return View(articleList);
        }
    }
}