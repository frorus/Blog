using Blog.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class LikeRepository : Repository<Like>
    {
        public LikeRepository(BlogDbContext db) : base(db)
        {

        }
    }
}
