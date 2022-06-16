using Blog.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(BlogDbContext db) : base(db)
        {

        }

        public async Task<IEnumerable<Tag>> GetAllTags()
        {
            var tags = Set.Include(t => t.Articles);

            return await tags.ToListAsync();
        }

        public async Task<Tag> GetTagById(Guid id)
        {
            var tags = Set.Include(t => t.Articles).ThenInclude(a => a.Tags);

            return await tags.FirstOrDefaultAsync(t => t.Id == id); ;
        }
    }
}
