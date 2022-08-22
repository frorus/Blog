using Blog.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class CommentLikeRepository : Repository<CommentLike>
    {
        public CommentLikeRepository(BlogDbContext db) : base(db)
        {

        }
    }
}
