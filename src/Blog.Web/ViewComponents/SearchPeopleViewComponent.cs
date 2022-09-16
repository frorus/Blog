using Blog.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.ViewComponents
{
    public class SearchPeopleViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SearchPeopleViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string search)
        {
            var users = await _userManager.Users.Where(x => x.Name.ToLower().Contains(search.ToLower()) ||
                                                             x.UserName.ToLower().Contains(search.ToLower()) ||
                                                             x.Bio.ToLower().Contains(search.ToLower())).ToListAsync();

            return View(users);
        }
    }
}