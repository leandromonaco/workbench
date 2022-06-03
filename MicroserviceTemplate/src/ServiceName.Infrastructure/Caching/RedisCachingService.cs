using ServiceName.Core.Common.Interfaces;

namespace ServiceName.Infrastructure.Caching
{
    /// <summary>
    /// https://github.com/redis/redis-om-dotnet
    /// </summary>
    public class RedisCachingService : ICachingService
    {
        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object value)
        {
            throw new NotImplementedException();
        }
    }
}
