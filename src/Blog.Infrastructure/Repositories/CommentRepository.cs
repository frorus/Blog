using Blog.Core.Models;
using Blog.Infrastructure.Data;

namespace Blog.Infrastructure.Repositories
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(BlogDbContext db) : base(db)
        {

        }
    }
}