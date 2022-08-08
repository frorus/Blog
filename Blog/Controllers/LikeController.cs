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
        public async Task<IActionResult> AddLike(string id)
        {
            var userId = _userManager.GetUserId(this.User);
            var articleFromDb = await _unitOfWork.GetRepository<Article>().GetByIdAsync(Guid.Parse(id));

            var like = new Like
            {
                UserId = Guid.Parse(userId),
                ArticleId = Guid.Parse(id)
            };

            await _unitOfWork.GetRepository<Like>().Create(like);

            return RedirectToAction("Details", "Article", new { id = id });
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteLike(string id)
        {
            var userId = _userManager.GetUserId(this.User);
            var articleFromDb = await _unitOfWork.GetRepository<Article>().GetByIdAsync(Guid.Parse(id));
            var likeFromDb = articleFromDb.Likes.FirstOrDefault(x => x.UserId == Guid.Parse(userId));

            if (likeFromDb == null)
            {
                return NotFound();
            }

            await _unitOfWork.GetRepository<Like>().Delete(likeFromDb);

            //TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Details", "Article", new { id = likeFromDb.ArticleId });
        }
    }
}
