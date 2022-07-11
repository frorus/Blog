using Blog.Data.UnitOfWork;
using Blog.Models.DB;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    }
}
