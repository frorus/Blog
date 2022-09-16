using Blog.Models.DB;

namespace Blog.Data.Repository
{
    public class CommentLikeRepository : Repository<CommentLike>
    {
        public CommentLikeRepository(BlogDbContext db) : base(db)
        {

        }
    }
}