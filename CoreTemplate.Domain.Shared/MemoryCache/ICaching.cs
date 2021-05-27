namespace CoreTemplate.Domain.Shared.MemoryCache
{
    public interface ICaching
    {
        object Get(string cacheKey);

        void Set(string cacheKey, object cacheValue,int catchTime);
    }
}
