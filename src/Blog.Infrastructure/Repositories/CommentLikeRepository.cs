using Blog.Core.Models;
using Blog.Infrastructure.Data;

namespace Blog.Infrastructure.Repositories
{
    public class CommentLikeRepository : Repository<CommentLike>
    {
        public CommentLikeRepository(BlogDbContext db) : base(db)
        {

        }
    }
}