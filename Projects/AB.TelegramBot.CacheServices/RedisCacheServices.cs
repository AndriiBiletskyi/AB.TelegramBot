using StackExchange.Redis;
using Newtonsoft.Json;

namespace AB.TelegramBot.CacheServices
{
    public class RedisCacheServices : ICacheServices
    {
        private IDatabase _database;

        public RedisCacheServices(string connectionString)
        {
            var redis = ConnectionMultiplexer.Connect(connectionString);
            _database = redis.GetDatabase();
        }

        public async Task<T?> ReadData<T>(string key)
        {
            var data = await _database.StringGetAsync(key);
            if (!data.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<T>(data);
            }

            return default;
        }

        public async Task WriteData<T>(string key, T value, DateTimeOffset? expirationTime = null)
        {
            if (expirationTime is not null)
            {
                var expTime = expirationTime.Value.DateTime.Subtract(DateTime.Now);
                await _database.StringSetAsync(key, JsonConvert.SerializeObject(value), expTime);
                return;
            }
            await _database.StringSetAsync(key, JsonConvert.SerializeObject(value));
        }
    }
}
