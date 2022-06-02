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
            var articles = Set.Include(a => a.Tags);

            return await articles.ToListAsync();
        }

        public async Task<Article> GetArticleById(Guid id)
        {
            var articles = Set.Include(a => a.Tags);

            return await articles.FirstOrDefaultAsync(a => a.Id == id); ;
        }
    }
}
