using AB.TelegramBot.Bank.Abstraction.Interfaces;
using AB.TelegramBot.Bank.Models;

namespace AB.TelegramBot.Services
{
    public interface IBanksServices
    {
        Task<IEnumerable<IBank>> GetBanks();
        Task<IEnumerable<Currency>> GetAccessibleCurrencies(IBank bank);
        IAsyncEnumerable<ExchangeRate> GetExchangeRates(ExchangeQuery exchangeQuery, IEnumerable<IBank> banks);
    }
}
