using AB.TelegramBot.Bank.Models;

namespace AB.TelegramBot.Bank.Abstraction.Interfaces
{
    public interface IBank
    {
        string Name { get; }
        string BankCode { get; }
        string Url { get; }
        Task<ExchangeRate> GetExchangeRate(ExchangeQuery exchangeQuery);
        Task<IEnumerable<Currency>> GetAccessibleCurrencies();
    }
}
