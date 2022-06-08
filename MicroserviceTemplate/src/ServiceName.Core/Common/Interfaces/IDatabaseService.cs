using ServiceName.Core.Model;

namespace ServiceName.Core.Common.Interfaces
{
    public interface IDatabaseService
    {
        Task<object> ExecuteQuery(QueryRead query);
        Task<bool> ExecuteQuery(QueryWrite query);
    }
}
