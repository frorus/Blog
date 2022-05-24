using Blog.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(BlogDbContext db) : base(db)
        {

        }
    }
}
