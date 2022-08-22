using Blog.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class ArticleRepository : Repository<Article>
    {
        public ArticleRepository(BlogDbContext db) : base(db)
        {

        }

        public async Task<IEnumerable<Article>> GetAllArticles()
        {
            var articles = Set.Include(a => a.Tags)
                              .Include(c => c.Comments);

            return await articles.ToListAsync();
        }

        public async Task<Article> GetArticleById(Guid id)
        {
            var articles = Set.Include(article => article.Tags)
                              .Include(article => article.Comments)
                                .ThenInclude(comments => comments.User);

            return await articles.FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
