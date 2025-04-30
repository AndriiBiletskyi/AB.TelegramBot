namespace AB.TelegramBot.Web.Clients
{
    public interface IWebClient
    {
        Task<T> GetAsync<T>(string url, string request);
    }
}
