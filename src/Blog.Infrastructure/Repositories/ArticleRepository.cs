using Blog.Core.Models;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
    public class ArticleRepository : Repository<Article>
    {
        public ArticleRepository(BlogDbContext db) : base(db)
        {

        }

        public IQueryable<Article> GetAllArticles()
        {
            var articles = Set.Include(article => article.Tags)
                              .Include(article => article.ArticleLikes)
                              .Include(article => article.Comments)
                              .Include(article => article.User);

            return articles.AsNoTracking();
        }

        public async Task<Article> GetArticleById(Guid id)
        {
            var article = Set.Include(article => article.Tags)
                             .Include(article => article.Comments)
                             .ThenInclude(comments => comments.User)
                             .Include(article => article.User);

            return await article.FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}