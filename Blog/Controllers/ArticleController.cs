using Blog.Data.Repository;
using Blog.Data.UnitOfWork;
using Blog.Models.DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public ArticleController(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Article> objArticleList = await _unitOfWork.GetRepository<Article>().GetAllAsync();
            return View(objArticleList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                var currentuser = User;
                var result = await _userManager.GetUserAsync(currentuser);

                article.UserId = Guid.Parse(result.Id);

                await _unitOfWork.GetRepository<Article>().Create(article);

                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to create");

            return View(article);
        }
    }
}
