namespace ServiceName.Core.Common.Interfaces
{
    public interface ICachingService
    {
        T Get<T>(string key);
        void Set(string key, object value);
    }
}