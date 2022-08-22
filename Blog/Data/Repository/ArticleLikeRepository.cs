using Blog.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class ArticleLikeRepository : Repository<ArticleLike>
    {
        public ArticleLikeRepository(BlogDbContext db) : base(db)
        {

        }
    }
}
