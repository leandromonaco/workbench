namespace ServiceName.Core.Common.Interfaces
{
    public interface IRepositoryService<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<bool> SaveAsync(Guid id, T obj);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
