using Blog.Data.Repository;
using Blog.Data.UnitOfWork;
using Blog.Models.DB;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArticleController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Article> articleList = await _unitOfWork.GetRepository<Article>().GetAllAsync();
            return View(articleList);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        //Create article
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(ArticleViewModel model)
        {
            var userId = _userManager.GetUserId(this.User);

            if (ModelState.IsValid)
            {
                var article = new Article
                {
                    Title = model.Title,
                    Text = model.Text,
                    AuthorUsername = _userManager.GetUserName(this.User),
                    UserId = Guid.Parse(userId),
                    User = await _userManager.FindByIdAsync(userId)
                };

                await _unitOfWork.GetRepository<Article>().Create(article);

                //TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to create");

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var articleFromDb = await _unitOfWork.GetRepository<Article>().GetByIdAsync(id);

            if (articleFromDb == null)
            {
                return NotFound();
            }

            return View(articleFromDb);
        }

        //Edit article
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.GetRepository<Article>().Update(article);
                //TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View(article);
        }
    }
}
