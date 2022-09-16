using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Infrastructure.Repositories;
using Blog.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Blog.Web.Controllers
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

        public async Task<IActionResult> Index(string sortOrder, int? page)
        {
            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var articleList = repository.GetAllArticles();

            if (sortOrder == "top_desc")
            {
                articleList = articleList.OrderByDescending(s => s.ArticleLikes.Count);
            }
            else
            {
                articleList = articleList.OrderByDescending(s => s.Date);
            }

            int pageSize = 20;
            int pageNumber = page ?? 1;

            return View(await articleList.ToPagedListAsync(pageNumber, pageSize));
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
            var userId = _userManager.GetUserId(User);
            var tagsFromDb = await _unitOfWork.GetRepository<Tag>().GetAllAsync();
            var selectedTags = model.Tags.Where(t => t.Selected).Select(t => t);

            //Get tag titles in wrong model case
            foreach (var tag in model.Tags)
            {
                tag.Text = tagsFromDb.Where(t => t.Id == Guid.Parse(tag.Value)).FirstOrDefault().Title;
            }

            if (selectedTags.Count() > 4)
            {
                ModelState.AddModelError("Tags", "Макс. число тегов - 4");
            }

            if (ModelState.IsValid)
            {
                var article = new Article
                {
                    Title = model.Title,
                    Text = model.Text,
                    UserId = Guid.Parse(userId),
                    User = await _userManager.FindByIdAsync(userId)
                };

                foreach (var selectedTag in selectedTags)
                {
                    var tag = await _unitOfWork.GetRepository<Tag>().GetByIdAsync(Guid.Parse(selectedTag.Value));
                    article.Tags.Add(tag);
                }

                await _unitOfWork.GetRepository<Article>().Create(article);

                return RedirectToAction("Details", new { id = article.Id });
            }

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

            if (articleFromDb == null || tagsFromDb == null)
            {
                return NotFound();
            }

            foreach (var tag in tagsFromDb)
            {
                selectedTags.Add(new SelectListItem(tag.Title, tag.Id.ToString()));
            }

            //Pre-check article current tags
            foreach (var tag in selectedTags)
            {
                foreach (var articleTag in articleFromDb.Tags)
                {
                    if (articleTag.Title == tag.Text)
                    {
                        tag.Selected = true;
                    }
                }
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

            var tagsFromDb = await _unitOfWork.GetRepository<Tag>().GetAllAsync();
            var selectedTags = model.Tags.Where(t => t.Selected).Select(t => t);

            //Get tag titles in wrong model case
            foreach (var tag in model.Tags)
            {
                tag.Text = tagsFromDb.Where(t => t.Id == Guid.Parse(tag.Value)).FirstOrDefault().Title;
            }

            if (selectedTags.Count() > 4)
            {
                ModelState.AddModelError("Tags", "Макс. число тегов - 4");
            }

            if (ModelState.IsValid)
            {
                articleFromDb.Text = model.Text;
                articleFromDb.Title = model.Title;
                articleFromDb.Tags.Clear();

                foreach (var t in selectedTags)
                {
                    var tag = await _unitOfWork.GetRepository<Tag>().GetByIdAsync(Guid.Parse(t.Value));
                    articleFromDb.Tags.Add(tag);
                }

                await _unitOfWork.GetRepository<Article>().Update(articleFromDb);

                TempData["success"] = "Статья успешно обновлена";

                return RedirectToAction("Details", new { id });
            }
            else
            {
                TempData["error"] = "Не удалось обновить статью";
            }

            return View(model);
        }

        //Get articles details
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
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

            //Get last 3 publications of article author
            var userArticles = await repository.GetAllArticles().Where(article => article.UserId == articleFromDb.UserId)
                                                                .OrderByDescending(article => article.Date)
                                                                .Take(3)
                                                                .ToListAsync();

            var article = new ArticleDetailsViewModel
            {
                Id = articleFromDb.Id,
                Date = articleFromDb.Date,
                Title = articleFromDb.Title,
                Text = articleFromDb.Text,
                UserId = articleFromDb.UserId,
                User = articleFromDb.User,
                UserArticles = userArticles,
                Comments = articleFromDb.Comments,
                Tags = articleFromDb.Tags,
                ArticleLikes = articleFromDb.ArticleLikes,
                Favourites = articleFromDb.Favourites
            };

            return View(article);
        }

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

            var deleteTask = _unitOfWork.GetRepository<Article>().Delete(articleFromDb);
            await deleteTask;

            if (deleteTask.IsCompletedSuccessfully)
            {
                TempData["success"] = "Статья успешно удалена";
            }
            else
            {
                TempData["error"] = "Не удалось удалить статью";
            }

            return RedirectToAction("Index");
        }
    }
}
