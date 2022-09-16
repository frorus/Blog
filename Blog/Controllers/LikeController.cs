using Blog.Data.UnitOfWork;
using Blog.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class LikeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public LikeController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        //Add like to article
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddLikeToArticle(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(this.User);

            if (String.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var like = new ArticleLike
            {
                UserId = Guid.Parse(userId),
                ArticleId = Guid.Parse(id),
            };

            var addLikeTask = _unitOfWork.GetRepository<ArticleLike>().Create(like);
            await addLikeTask;

            if (!addLikeTask.IsCompletedSuccessfully)
            {
                TempData["error"] = "Упс! Что-то пошло не так";
            }

            return RedirectToAction("Details", "Article", new { id });
        }

        //Remove like from article
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteLikeFromArticle(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(this.User);

            if (String.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var articleFromDb = await _unitOfWork.GetRepository<Article>().GetByIdAsync(Guid.Parse(id));

            if (articleFromDb == null)
            {
                return NotFound();
            }

            var likeFromDb = articleFromDb.ArticleLikes.FirstOrDefault(x => x.UserId == Guid.Parse(userId));

            if (likeFromDb == null)
            {
                return NotFound();
            }

            var deleteLikeTask = _unitOfWork.GetRepository<ArticleLike>().Delete(likeFromDb);
            await deleteLikeTask;

            if (!deleteLikeTask.IsCompletedSuccessfully)
            {
                TempData["error"] = "Упс! Что-то пошло не так";
            }

            return RedirectToAction("Details", "Article", new { id });
        }

        //Add like to comment
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddLikeToComment(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(this.User);

            if (String.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var commentFromDb = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(Guid.Parse(id));

            if (commentFromDb == null)
            {
                return NotFound();
            }

            var like = new CommentLike
            {
                UserId = Guid.Parse(userId),
                CommentId = Guid.Parse(id),
            };

            var addCommentLikeTask = _unitOfWork.GetRepository<CommentLike>().Create(like);
            await addCommentLikeTask;

            if (!addCommentLikeTask.IsCompletedSuccessfully)
            {
                TempData["error"] = "Упс! Что-то пошло не так";
            }

            return RedirectToAction("Details", "Article", new { id = commentFromDb.ArticleId });
        }

        //Remove like from comment
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteLikeFromComment(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(this.User);

            if (String.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var commentFromDb = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(Guid.Parse(id));

            if (commentFromDb == null)
            {
                return NotFound();
            }

            var likeFromDb = commentFromDb.CommentLikes.FirstOrDefault(x => x.UserId == Guid.Parse(userId));

            if (likeFromDb == null)
            {
                return NotFound();
            }

            var deleteCommentLikeTask = _unitOfWork.GetRepository<CommentLike>().Delete(likeFromDb);
            await deleteCommentLikeTask;

            if (!deleteCommentLikeTask.IsCompletedSuccessfully)
            {
                TempData["error"] = "Упс! Что-то пошло не так";
            }

            return RedirectToAction("Details", "Article", new { id = commentFromDb.ArticleId });
        }
    }
}