namespace ProductApi.Application.Interfaces;

public interface ICacheService
{
    Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null);
    Task RemoveAsync(string cacheKey);
}