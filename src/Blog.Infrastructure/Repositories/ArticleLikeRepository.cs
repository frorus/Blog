using Blog.Core.Models;
using Blog.Infrastructure.Data;

namespace Blog.Infrastructure.Repositories
{
    public class ArticleLikeRepository : Repository<ArticleLike>
    {
        public ArticleLikeRepository(BlogDbContext db) : base(db)
        {

        }
    }
}