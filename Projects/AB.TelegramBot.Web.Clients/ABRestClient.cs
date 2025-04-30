using AB.TelegramBot.CacheServices;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace AB.TelegramBot.Web.Clients
{
    public class ABRestClient : IWebClient
    {
        private readonly ILogger? _logger;
        private readonly ICacheServices? _cacheServices;

        public ABRestClient(ILoggerFactory? logger = null, 
                            ICacheServices? cacheServices = null)
        {
            _logger = logger?.CreateLogger<ABRestClient>();
            _cacheServices = cacheServices;
        }

        public async Task<T> GetAsync<T>(string url, string request)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrWhiteSpace(request))
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (_cacheServices is not null)
            {
                var cache = await _cacheServices.ReadData<T>($"{url}{request}");
                if (cache is not null)
                {
                    _logger?.LogInformation($"Return data from cache.");
                    return cache;
                }
            }

            using var client = new RestClient(url);
            _logger?.LogInformation($"Create new instance of RestClient for url - [{url}]");

            if (client is not null)
            {
                var req = new RestRequest(request);
                var response = await client.ExecuteAsync<T>(req);

                if (response is not null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (response.Data is not null)
                    {
                        if (_cacheServices is not null)
                        {
                            await _cacheServices.WriteData($"{url}{request}", response.Data);
                            _logger?.LogInformation("Wrote data to cache.");
                        }
                        return response.Data;
                    }

                    _logger?.LogError($"RestClient returned response for request - [{url}{request}] without data.");
                    throw new Exception($"Error. [{url}] responded without data.");
                }

            }

            _logger?.LogError($"Client for url - {url} does not exist.");
            throw new Exception($"Client for url - {url} does not exist.");
        }
    }
}
