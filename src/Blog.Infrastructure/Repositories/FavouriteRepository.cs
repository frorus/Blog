using Blog.Core.Models;
using Blog.Infrastructure.Data;

namespace Blog.Infrastructure.Repositories
{
    public class FavouriteRepository : Repository<Favourite>
    {
        public FavouriteRepository(BlogDbContext db) : base(db)
        {

        }
    }
}