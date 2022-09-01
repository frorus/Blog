using Blog.Data.Repository;
using Blog.Data.UnitOfWork;
using Blog.Models.DB;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public async Task<IActionResult> Index(string sortOrder)
        {
            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            IEnumerable<Article> articleList = await repository.GetAllArticles();

            if (sortOrder == "top_desc")
            {
                articleList = articleList.OrderByDescending(s => s.ArticleLikes.Count);
            }
            else
            {
                articleList = articleList.OrderByDescending(s => s.Date);
            }

            return View(articleList);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var tagsFromDb = await _unitOfWork.GetRepository<Tag>().GetAllAsync();
            var selectedTags = new List<SelectListItem>();

            foreach (var tag in tagsFromDb)
            {
                selectedTags.Add(new SelectListItem(tag.Title, tag.Id.ToString()));
            }

            var articleModel = new ArticleViewModel
            {
                Tags = selectedTags
            };

            return View(articleModel);
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
                    UserId = Guid.Parse(userId),
                    User = await _userManager.FindByIdAsync(userId)
                };

                var selectedTags = model.Tags.Where(t => t.Selected).Select(t => t);

                foreach (var t in selectedTags)
                {
                    var tag = await _unitOfWork.GetRepository<Tag>().GetByIdAsync(Guid.Parse(t.Value));
                    article.Tags.Add(tag);
                }

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

            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var articleFromDb = await repository.GetArticleById(id);

            var tagsFromDb = await _unitOfWork.GetRepository<Tag>().GetAllAsync();
            var selectedTags = new List<SelectListItem>();

            foreach (var tag in tagsFromDb)
            {
                selectedTags.Add(new SelectListItem(tag.Title, tag.Id.ToString()));
            }

            foreach (var tag in selectedTags)
            {
                foreach (var articleTag in articleFromDb.Tags)
                {
                    if(articleTag.Title == tag.Text)
                    {
                        tag.Selected = true;
                    }
                }
            }

            if (articleFromDb == null)
            {
                return NotFound();
            }

            var articleModel = new ArticleViewModel
            {
                Title = articleFromDb.Title,
                Text = articleFromDb.Text,
                Tags = selectedTags
            };

            return View(articleModel);
        }

        //Edit article
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id, ArticleViewModel model)
        {
            //var articleFromDb = await _unitOfWork.GetRepository<Article>().GetByIdAsync(id);

            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var articleFromDb = await repository.GetArticleById(id);

            if (articleFromDb == null)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            //    articleFromDb.Text = model.Text;
            //    articleFromDb.Title = model.Title;

            //    await _unitOfWork.GetRepository<Article>().Update(articleFromDb);
            //    //TempData["success"] = "Category updated successfully";
            //    return RedirectToAction("Index");
            //}

            articleFromDb.Text = model.Text;
            articleFromDb.Title = model.Title;
            articleFromDb.Tags.Clear();

            var selectedTags = model.Tags.Where(t => t.Selected).Select(t => t);

            foreach (var t in selectedTags)
            {
                var tag = await _unitOfWork.GetRepository<Tag>().GetByIdAsync(Guid.Parse(t.Value));
                articleFromDb.Tags.Add(tag);
            }

            await _unitOfWork.GetRepository<Article>().Update(articleFromDb);

            return RedirectToAction("Details", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            //var articleFromDb = await _unitOfWork.GetRepository<Article>().GetByIdAsync(id);
            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var articleFromDb = await repository.GetArticleById(id);

            if (articleFromDb == null)
            {
                return NotFound();
            }

            return View(articleFromDb);
        }


        ////Edit article
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public async Task<IActionResult> Details(Guid id, ArticleViewModel model)
        //{
        //    var articleFromDb = await _unitOfWork.GetRepository<Article>().GetByIdAsync(id);

        //    if (articleFromDb == null)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        articleFromDb.Text = model.Text;
        //        articleFromDb.Title = model.Title;

        //        await _unitOfWork.GetRepository<Article>().Update(articleFromDb);
        //        //TempData["success"] = "Category updated successfully";
        //        return RedirectToAction("Index");
        //    }

        //    return View(model);
        //}

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var articleFromDb = await repository.GetArticleById(id);

            if (articleFromDb == null)
            {
                return NotFound();
            }

            return View(articleFromDb);
        }

        //Delete article
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeletePOST(Guid id)
        {
            var articleFromDb = await _unitOfWork.GetRepository<Article>().GetByIdAsync(id);

            if (articleFromDb == null)
            {
                return NotFound();
            }

            await _unitOfWork.GetRepository<Article>().Delete(articleFromDb);

            //TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
