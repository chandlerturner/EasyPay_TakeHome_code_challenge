using Microsoft.Extensions.Caching.Memory;
using ProductApi.Application.Interfaces;

namespace ProductApi.Infrastructure.Services;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
    {
        if (_cache.TryGetValue(cacheKey, out T cacheEntry))
        {
            return cacheEntry;
        }

        var result = await factory();
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiration,
            SlidingExpiration = slidingExpiration
        };

        _cache.Set(cacheKey, result, cacheOptions);

        return result;
    }

    public Task RemoveAsync(string cacheKey)
    {
        _cache.Remove(cacheKey);
        return Task.CompletedTask;
    }
}