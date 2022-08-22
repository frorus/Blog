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
            var userId = _userManager.GetUserId(this.User);
            //var articleFromDb = await _unitOfWork.GetRepository<Article>().GetByIdAsync(Guid.Parse(id));

            var like = new ArticleLike
            {
                UserId = Guid.Parse(userId),
                ArticleId = Guid.Parse(id),
            };

            await _unitOfWork.GetRepository<ArticleLike>().Create(like);

            return RedirectToAction("Details", "Article", new { id });
        }

        //Remove like from article
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteLikeFromArticle(string id)
        {
            var userId = _userManager.GetUserId(this.User);
            var articleFromDb = await _unitOfWork.GetRepository<Article>().GetByIdAsync(Guid.Parse(id));
            var likeFromDb = articleFromDb.ArticleLikes.FirstOrDefault(x => x.UserId == Guid.Parse(userId));

            if (likeFromDb == null)
            {
                return NotFound();
            }

            await _unitOfWork.GetRepository<ArticleLike>().Delete(likeFromDb);

            //TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Details", "Article", new { id });
        }

        //Add like to comment
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddLikeToComment(string id)
        {
            var userId = _userManager.GetUserId(this.User);
            var commentFromDb = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(Guid.Parse(id));
            var like = new CommentLike
            {
                UserId = Guid.Parse(userId),
                CommentId = Guid.Parse(id),
            };

            await _unitOfWork.GetRepository<CommentLike>().Create(like);

            return RedirectToAction("Details", "Article", new { id = commentFromDb.ArticleId });
        }

        //Remove like from comment
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteLikeFromComment(string id)
        {
            var userId = _userManager.GetUserId(this.User);
            var commentFromDb = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(Guid.Parse(id));
            var likeFromDb = commentFromDb.CommentLikes.FirstOrDefault(x => x.UserId == Guid.Parse(userId));

            if (likeFromDb == null)
            {
                return NotFound();
            }

            await _unitOfWork.GetRepository<CommentLike>().Delete(likeFromDb);

            //TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Details", "Article", new { id = commentFromDb.ArticleId });
        }
    }
}