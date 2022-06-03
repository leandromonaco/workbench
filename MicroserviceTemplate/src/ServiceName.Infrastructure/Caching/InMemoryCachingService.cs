using Microsoft.Extensions.Caching.Memory;
using ServiceName.Core.Common.Interfaces;

namespace ServiceName.Infrastructure.Caching
{
    /// <summary>
    ///https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-6.0
    /// </summary>
    public class InMemoryCachingService : ICachingService
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCachingService()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }
        
        public T Get<T>(string key)
        {
            return (T)_memoryCache.Get(key);
        }

        public void Set(string key, object value)
        {
            _memoryCache.Set(key, value);
        }
    }
}
