using Blog.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class FavouriteRepository : Repository<Favourite>
    {
        public FavouriteRepository(BlogDbContext db) : base(db)
        {

        }
    }
}
