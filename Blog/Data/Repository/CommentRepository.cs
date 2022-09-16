using Blog.Models.DB;

namespace Blog.Data.Repository
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(BlogDbContext db) : base(db)
        {

        }
    }
}