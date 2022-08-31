using Blog.Extensions;
using Blog.Models.DB;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileService _fileService;

        public AccountController(UserManager<ApplicationUser> userManager, IFileService fileService)
        {
            _userManager = userManager;
            _fileService = fileService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["UserEmail"] = user.Email;

            if (user == null)
            {
                return NotFound();
            }

            var model = new UserProfileSettingsViewModel
            {
                Email = user.Email,
                Username = user.UserName,
                Name = user.Name,
                ImagePath = user.ImagePath,
                Website = user.Website,
                Location = user.Location,
                Bio = user.Bio,
                Learning = user.Learning,
                Skills = user.Skills,
                Work = user.Work,
                Education = user.Education
            };

            return View(model);
        }

        //Edit profile settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditProfileSettings(UserProfileSettingsViewModel model, IFormFile file)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var fileName = await _fileService.UploadImage(file);
                    user.ImagePath = fileName;
                }

                user.Email = model.Email;
                user.UserName = model.Username;
                user.Name = model.Name;
                user.Website = model.Website;
                user.Location = model.Location;
                user.Bio = model.Bio;
                user.Learning = model.Learning;
                user.Skills = model.Skills;
                user.Work = model.Work;
                user.Education = model.Education;

                await _userManager.UpdateAsync(user);
                //TempData["success"] = "Category updated successfully";
                return RedirectToAction("Settings");
            }

            return RedirectToAction("Settings");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditAccountSettings(UserAccountSettingsViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.PasswordCurrent, model.PasswordNew);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Settings");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.ToString());
                    }

                    return RedirectToAction("Settings");
                }

                return NotFound();
            }

            return RedirectToAction("Settings"); ;
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
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
    }
}