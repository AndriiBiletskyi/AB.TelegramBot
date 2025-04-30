using AB.TelegramBot.Bank.Models;
using AB.TelegramBot.Resources.Strings;
using AB.TelegramBot.Services;
using AB.TelegramBot.Web.MainApp.Extensions;

namespace AB.TelegramBot.Web.MainApp.Services
{
    public class ExchangeServices
    {
        private readonly IBanksServices _banksServices;
        private readonly ILogger<ExchangeServices> _logger;

        public ExchangeServices(IBanksServices banksServices,
                                ILoggerFactory logger)
        {
            _banksServices = banksServices;
            _logger = logger.CreateLogger<ExchangeServices>();
        }

        public async Task<IEnumerable<string>> GetRates(ExchangeQuery exchangeQuery)
        {
            try
            {
                var rates = _banksServices.GetExchangeRates(exchangeQuery, await _banksServices.GetBanks());
                var list = new List<string>();
                await foreach (var rate in rates)
                {
                    list.Add(rate.ToTelegramBotString());
                }
                return list;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error - {ex.Message}");
                return new List<string>() { StringResources.SomethingWentWrongTryLater };
            }
        }
    }
}
