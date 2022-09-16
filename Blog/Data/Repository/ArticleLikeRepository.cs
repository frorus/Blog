using Blog.Models.DB;

namespace Blog.Data.Repository
{
    public class ArticleLikeRepository : Repository<ArticleLike>
    {
        public ArticleLikeRepository(BlogDbContext db) : base(db)
        {

        }
    }
}