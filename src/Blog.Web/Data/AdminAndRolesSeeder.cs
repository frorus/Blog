﻿using Blog.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog.Web.Data
{
    public static class AdminAndRolesSeeder
    {
        public static async Task SeedAdmin(IServiceProvider serviceProvider)
        {
            using (UserManager<ApplicationUser> _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>())
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@email.com",
                    Email = "admin@email.com",
                    ImagePath = "default_avatar.png",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                };

                if (!_userManager.Users.Any(r => r.UserName == user.UserName))
                {
                    var result = await _userManager.CreateAsync(user, "P@ssw0rd!");

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }
        }

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            using (RoleManager<IdentityRole> _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>())
            {
                string[] roles = new string[] { "Admin", "Moderator", "Basic" };

                foreach (string role in roles)
                {
                    if (!_roleManager.Roles.Any(r => r.Name == role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
        }
    }
}