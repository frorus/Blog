using Blog.Data.Repository;
using Blog.Data.UnitOfWork;
using Blog.Models.DB;
using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("Profile/{id}")]
        public async Task<IActionResult> GetUserProfile(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var user = await _userManager.Users.Include(user => user.Articles)
                                               .ThenInclude(article => article.Tags)
                                               .Include(user => user.Comments)
                                               .FirstOrDefaultAsync(user => user.Id == id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var model = new UserProfileViewModel
            {
                ImagePath = user.ImagePath,
                Name = user.Name,
                Bio = user.Bio,
                Location = user.Location,
                JoinedDate = user.JoinedDate,
                Email = user.Email,
                Website = user.Website,
                Education = user.Education,
                Work = user.Work,
                Learning = user.Learning,
                Skills = user.Skills,
                Comments = user.Comments.Count,
                Articles = user.Articles.OrderByDescending(article => article.Date).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserReadingList(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var readingList = await repository.GetAllArticles().Where(article => article.Favourites.Any(favourite => favourite.UserId == id)).ToListAsync();
            var tagList = readingList.SelectMany(article => article.Tags).Select(tag => tag.Title).Distinct().ToList();

            var model = new ReadingListViewModel
            {
                UserId = id,
                Articles = readingList,
                Tags = tagList
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserReadingListByTag(Guid id, string tag)
        {
            if (id == Guid.Empty || string.IsNullOrEmpty(tag))
            {
                return NotFound();
            }

            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var readingList = await repository.GetAllArticles().Where(article => article.Favourites.Any(favourite => favourite.UserId == id) &&
                                                                                 article.Tags.Any(z => z.Title == tag)).ToListAsync();

            return PartialView("~/Views/Shared/ReadingList/_ReadingListPartial.cshtml", readingList);
        }

        [HttpGet]
        public async Task<int> GetUserReadingListCount(Guid id)
        {

            var repository = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var readingList = await repository.GetAllArticles().Where(article => article.Favourites.Any(favourite => favourite.UserId == id)).ToListAsync();

            return readingList.Count;
        }
    }
}