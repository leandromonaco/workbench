namespace ServiceName.Core.Common.Interfaces
{
    public interface IRepositoryService<T>
    {
        Task<T> GetAsync(Guid id);
        Task<bool> SaveAsync(Guid id, T obj);
    }
}
