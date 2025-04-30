using AB.TelegramBot.Bank.Abstraction.Interfaces;
using RestSharp;

namespace AB.TelegramBot.Factories
{
    public class RestClientFactory : IRestClientFactory
    {
        public Dictionary<IBank, string> RestClients = new();

        public IRestClient CreateRestClient(Type type)
        {
            foreach (var key in RestClients.Keys)
            {
                if (key.GetType() == type)
                {
                    return new RestClient(RestClients[key]);
                }
            }

            throw new ArgumentException(nameof(type));
        }
    }

    public static class RestClientFactoryExtensions
    {
        public static RestClientFactory RegisterBank(this RestClientFactory restClient, IBank bank, string url)
        {
            restClient.RestClients.Add(bank, url);
            return restClient;
        }
    }
}
