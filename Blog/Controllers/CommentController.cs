using Blog.Data.Repository;
using Blog.Data.UnitOfWork;
using Blog.Models.DB;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Blog.Controllers
{
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        //Add comment to article
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(CommentViewModel model)
        {
            var userId = _userManager.GetUserId(this.User);
            var articleFromDb = await _unitOfWork.GetRepository<Article>().GetByIdAsync(model.ArticleId);

            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    Content = model.Content!,
                    UserId = Guid.Parse(userId),
                    User = await _userManager.FindByIdAsync(userId),
                    ArticleId = model.ArticleId,
                    Article = articleFromDb
                };

                await _unitOfWork.GetRepository<Comment>().Create(comment);

                // TempData["success"] = "Category created successfully";
            }

            //ModelState.AddModelError("", "Failed to create");

            return RedirectToAction("Details", "Article", new { id = model.ArticleId });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
			if (id == Guid.Empty)
			{
				return NotFound();
			}

			var commentFromDb = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(id);

            var model = new CommentEditViewModel
            {
                Id = commentFromDb.Id,
                Content = commentFromDb.Content,
            };

            return PartialView("_CommentEditPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(CommentEditViewModel model)
        {
            //var repository = _unitOfWork.GetRepository<Comment>() as ArticleRepository;
            //var articleFromDb = await repository.GetArticleById(id);

            var commentFromDb = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(model.Id);

            if (commentFromDb == null)
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

            commentFromDb.Content = model.Content!;

            await _unitOfWork.GetRepository<Comment>().Update(commentFromDb);

            return RedirectToAction("Details", "Article", new { id = commentFromDb.ArticleId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var commentFromDb = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(id);

            var model = new CommentEditViewModel
            {
                Id = commentFromDb.Id,
                Content = commentFromDb.Content,
            };

            return PartialView("_CommentDeletePartial", model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(Guid id)
        {
            var commentFromDb = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(id);

            if (commentFromDb == null)
            {
                return NotFound();
            }

            await _unitOfWork.GetRepository<Comment>().Delete(commentFromDb);

            //TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Details", "Article", new { id = commentFromDb.ArticleId });
        }
    }
}
