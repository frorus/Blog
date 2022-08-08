using Blog.Data.UnitOfWork;
using Blog.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class FavouriteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavouriteController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        //Add article to favourite
        [HttpPost, ActionName("Add")]
        [Authorize]
        public async Task<IActionResult> AddToFavourite(string id)
        {
            var userId = _userManager.GetUserId(this.User);
            //var articleFromDb = await _unitOfWork.GetRepository<Article>().GetByIdAsync(Guid.Parse(id));

            var favourite = new Favourite
            {
                UserId = Guid.Parse(userId),
                ArticleId = Guid.Parse(id)
            };

            await _unitOfWork.GetRepository<Favourite>().Create(favourite);

            return RedirectToAction("Details", "Article", new { id = id });
        }

        //Delete article from favourite
        [HttpPost, ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteFromFavourite(string id)
        {
            var userId = _userManager.GetUserId(this.User);
            var articleFromDb = await _unitOfWork.GetRepository<Article>().GetByIdAsync(Guid.Parse(id));
            var favouriteFromDb = articleFromDb.Favourites.FirstOrDefault(x => x.UserId == Guid.Parse(userId));

            if (favouriteFromDb == null)
            {
                return NotFound();
            }

            await _unitOfWork.GetRepository<Favourite>().Delete(favouriteFromDb);

            //TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Details", "Article", new { id = id });
        }
    }
}
