using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext _db;

        public DbSet<T> Set
        {
            get;
            private set;
        }

        public Repository(BlogDbContext db)
        {
            _db = db;
            var set = _db.Set<T>();
            set.Load();

            Set = set;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Set.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid guid)
        {
            return await Set.FindAsync(guid);
        }

        public async Task Create(T item)
        {
            await Set.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public Task Update(T item)
        {
            Set.Update(item);
            return _db.SaveChangesAsync();
        }

        public Task Delete(T item)
        {
            Set.Remove(item);
            return _db.SaveChangesAsync();
        }
    }
}
