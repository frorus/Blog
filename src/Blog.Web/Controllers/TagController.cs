using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Infrastructure.Repositories;
using Blog.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class TagController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var repository = _unitOfWork.GetRepository<Tag>() as TagRepository;
            IEnumerable<Tag> tagList = await repository.GetAllTags();

            return View(tagList.OrderByDescending(x => x.Articles.Count));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        //Create tag
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Tag model)
        {
            var repository = await _unitOfWork.GetRepository<Tag>().GetAllAsync();

            if (repository.Any(tag => tag.Title.ToLower() == model.Title.ToLower()))
            {
                ModelState.AddModelError("Title", "Такой тег уже существует");
            }

            if (ModelState.IsValid)
            {
                var tag = new Tag
                {
                    Title = model.Title,
                    Description = model.Description
                };

                await _unitOfWork.GetRepository<Tag>().Create(tag);

                TempData["success"] = "Тег успешно добавлен";

                return RedirectToAction("Index");
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

            var tagFromDb = await _unitOfWork.GetRepository<Tag>().GetByIdAsync(id);

            if (tagFromDb == null)
            {
                return NotFound();
            }

            var tagModel = new TagViewModel
            {
                Title = tagFromDb.Title,
                Description = tagFromDb.Description
            };

            return View(tagModel);
        }

        //Edit tag
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id, TagViewModel model)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var tagFromDb = await _unitOfWork.GetRepository<Tag>().GetByIdAsync(id);

            if (tagFromDb == null)
            {
                return NotFound();
            }

            var repository = await _unitOfWork.GetRepository<Tag>().GetAllAsync();

            if (tagFromDb.Title != model.Title && repository.Any(tag => tag.Title.ToLower() == model.Title.ToLower()))
            {
                ModelState.AddModelError("Title", "Такой тег уже существует");
            }

            if (ModelState.IsValid)
            {
                tagFromDb.Title = model.Title;
                tagFromDb.Description = model.Description;

                await _unitOfWork.GetRepository<Tag>().Update(tagFromDb);

                TempData["success"] = "Тег успешно обновлен";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Не удалось обновить тег";
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id, string sortOrder)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var repository = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tagFromDb = await repository.GetTagById(id);

            if (tagFromDb == null)
            {
                return NotFound();
            }

            if (sortOrder == "top_desc")
            {
                tagFromDb.Articles = tagFromDb.Articles.OrderByDescending(s => s.ArticleLikes.Count).ToList();
            }
            else
            {
                tagFromDb.Articles = tagFromDb.Articles.OrderByDescending(s => s.Date).ToList();
            }

            return View(tagFromDb);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var tagFromDb = await _unitOfWork.GetRepository<Tag>().GetByIdAsync(id);

            if (tagFromDb == null)
            {
                return NotFound();
            }

            return View(tagFromDb);
        }

        //Delete tag
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var tagFromDb = await _unitOfWork.GetRepository<Tag>().GetByIdAsync(id);

            if (tagFromDb == null)
            {
                return NotFound();
            }

            var deleteTask = _unitOfWork.GetRepository<Tag>().Delete(tagFromDb);
            await deleteTask;

            if (deleteTask.IsCompletedSuccessfully)
            {
                TempData["success"] = "Тег успешно удален";
            }
            else
            {
                TempData["error"] = "Не удалось удалить тег";
            }

            return RedirectToAction("Index");
        }
    }
}