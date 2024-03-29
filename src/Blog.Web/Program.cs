using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Core.Services;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Repositories;
using Blog.Infrastructure.Services;
using Blog.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ))
    .AddUnitOfWork()
    .AddCustomRepository<Article, ArticleRepository>()
    .AddCustomRepository<Tag, TagRepository>()
    .AddCustomRepository<Comment, CommentRepository>()
    .AddCustomRepository<ArticleLike, ArticleLikeRepository>()
    .AddCustomRepository<CommentLike, CommentLikeRepository>()
    .AddCustomRepository<Favourite, FavouriteRepository>()
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BlogDbContext>();

builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddRazorPages();

//builder.Services.Configure<IdentityOptions>(options =>
//{
//    // Password settings.
//    options.Password.RequireDigit = true;
//    options.Password.RequireLowercase = true;
//    options.Password.RequireNonAlphanumeric = true;
//    options.Password.RequireUppercase = true;
//    options.Password.RequiredLength = 6;
//    options.Password.RequiredUniqueChars = 1;

//    // Lockout settings.
//    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
//    options.Lockout.MaxFailedAccessAttempts = 5;
//    options.Lockout.AllowedForNewUsers = true;

//    // User settings.
//    options.User.AllowedUserNameCharacters =
//    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
//    options.User.RequireUniqueEmail = false;
//});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    //options.Cookie.HttpOnly = true;
    //options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Login/Login";
    //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    //options.SlidingExpiration = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await AdminAndRolesSeeder.SeedRoles(services);
    await AdminAndRolesSeeder.SeedAdmin(services);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Article}/{action=Index}/{id?}");

app.Run();
