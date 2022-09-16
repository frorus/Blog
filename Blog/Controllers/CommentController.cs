using Blog.Data.UnitOfWork;
using Blog.Models.DB;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

            if (String.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var articleFromDb = await _unitOfWork.GetRepository<Article>().GetByIdAsync(model.ArticleId);

            if (articleFromDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    Content = model.Content,
                    UserId = Guid.Parse(userId),
                    User = await _userManager.FindByIdAsync(userId),
                    ArticleId = model.ArticleId,
                    Article = articleFromDb
                };

                await _unitOfWork.GetRepository<Comment>().Create(comment);

                TempData["success"] = "Комментарий успешно добавлен";
            }
            else
            {
                TempData["error"] = "Не удалось добавить комментарий";
            }

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

            if (commentFromDb == null)
            {
                return NotFound();
            }

            var model = new CommentEditViewModel
            {
                Id = commentFromDb.Id,
                Content = commentFromDb.Content,
            };

            return PartialView("~/Views/Shared/Comment/_CommentEditPartial.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(CommentEditViewModel model)
        {
            var commentFromDb = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(model.Id);

            if (commentFromDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                commentFromDb.Content = model.Content;

                await _unitOfWork.GetRepository<Comment>().Update(commentFromDb);

                TempData["success"] = "Комментарий успешно обновлен";
            }
            else
            {
                TempData["error"] = "Не удалось обновить комментарий";
            }

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

            if (commentFromDb == null)
            {
                return NotFound();
            }

            var model = new CommentEditViewModel
            {
                Id = commentFromDb.Id,
                Content = commentFromDb.Content,
            };

            return PartialView("~/Views/Shared/Comment/_CommentDeletePartial.cshtml", model);
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

            var deleteTask = _unitOfWork.GetRepository<Comment>().Delete(commentFromDb);
            await deleteTask;

            if (deleteTask.IsCompletedSuccessfully)
            {
                TempData["success"] = "Комментарий успешно удален";
            }
            else
            {
                TempData["error"] = "Не удалось удалить комментарий";
            }

            return RedirectToAction("Details", "Article", new { id = commentFromDb.ArticleId });
        }
    }
}
