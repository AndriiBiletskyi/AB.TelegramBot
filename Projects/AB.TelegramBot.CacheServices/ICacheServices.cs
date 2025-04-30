namespace AB.TelegramBot.CacheServices
{
    public interface ICacheServices
    {
        Task<T?> ReadData<T>(string key);
        Task WriteData<T>(string key, T value, DateTimeOffset? expirationTime = null);
    }
}
