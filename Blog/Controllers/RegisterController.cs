using Blog.Models.DB;
using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            ViewData["UsersCount"] = _userManager.Users.Count();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var checkUser = await _userManager.FindByEmailAsync(model.EmailReg);

            if (checkUser != null)
            {
                ModelState.AddModelError("EmailReg", "Пользователь с такой почтой уже зарегистрирован");
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.EmailReg,
                    Email = model.EmailReg,
                };

                var result = await _userManager.CreateAsync(user, model.PasswordReg);

                if (result.Succeeded)
                {
                    user.ImagePath = "default_avatar.png";
                    await _signInManager.SignInAsync(user, false);
                    await _userManager.AddToRoleAsync(user, "Basic");

                    return RedirectToAction("Index", "Article");
                }
            }

            return View(model);
        }
    }
}