using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
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

        //Get user settings
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);

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
            var checkUserByEmail = await _userManager.FindByEmailAsync(model.Email);
            var checkUserByUsername = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return NotFound();
            }

            if (checkUserByEmail != null && user.Email != model.Email)
            {
                ModelState.AddModelError("Email", "Почта уже существует");
            }

            if (checkUserByUsername != null && user.UserName != model.Username)
            {
                ModelState.AddModelError("Username", "Username уже существует");
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

                TempData["success"] = "Настройки профиля успешно обновлены.";
            }

            model.ImagePath = user.ImagePath;

            return View("Settings", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditAccountSettings(UserAccountSettingsViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.PasswordCurrent, model.PasswordNew);

                if (result.Succeeded)
                {
                    TempData["success"] = "Пароль успешно обновлен.";
                }
                else
                {
                    TempData["error"] = "Не удалось обновить пароль.";
                }
            }

            return RedirectToAction("Settings");
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
    }
}