namespace Blog.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid guid);
        Task Create(T item);
        Task Update(T item);
        Task Delete(T item);
    }
}