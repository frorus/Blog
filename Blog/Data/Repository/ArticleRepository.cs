using Blog.Models.DB;

namespace Blog.Data.Repository
{
    public class ArticleRepository : Repository<Article>
    {
        public ArticleRepository(BlogDbContext db) : base(db)
        {

        }
    }
}
