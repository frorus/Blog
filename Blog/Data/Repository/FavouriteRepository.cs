using Blog.Models.DB;

namespace Blog.Data.Repository
{
    public class FavouriteRepository : Repository<Favourite>
    {
        public FavouriteRepository(BlogDbContext db) : base(db)
        {

        }
    }
}