namespace Blog.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task Create(T item);
        Task Update(T item);
        Task Delete(T item);
    }
}