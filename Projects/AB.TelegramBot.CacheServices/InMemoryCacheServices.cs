using Microsoft.Extensions.Caching.Memory;

namespace AB.TelegramBot.CacheServices
{
    public class InMemoryCacheServices : ICacheServices
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheServices()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public async Task<T?> ReadData<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public Task WriteData<T>(string key, T value, DateTimeOffset? expirationTime = null)
        {
            if (expirationTime is not null)
            {
                _memoryCache.Set(key, value, (DateTimeOffset)expirationTime);
                return Task.CompletedTask;
            }

            _memoryCache.Set(key, value);
            return Task.CompletedTask;
        }
    }
}
