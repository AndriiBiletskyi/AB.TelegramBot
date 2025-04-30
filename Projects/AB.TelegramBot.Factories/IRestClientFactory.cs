using RestSharp;

namespace AB.TelegramBot.Factories
{
    public interface IRestClientFactory
    {
        IRestClient CreateRestClient(Type type);
    }
}
