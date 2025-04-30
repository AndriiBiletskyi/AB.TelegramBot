using AB.TelegramBot.Bank.Abstraction.Interfaces;
using AB.TelegramBot.Bank.Models;
using Microsoft.Extensions.Logging;

namespace AB.TelegramBot.Services
{
    public class BanksServices : IBanksServices
    {
        private readonly ILogger<BanksServices>? _logger;
        private readonly List<IBank> _banks;

        public BanksServices(IEnumerable<IBank> banks,
                             ILoggerFactory? logger = null)
        {
            _logger = logger?.CreateLogger<BanksServices>();
            _logger?.LogInformation("Create an instance of BanksServices.");
            _banks = new(banks);
            _logger?.LogInformation($"Get all banks from DI:\n{string.Join('\n', _banks.Select(b => $"{b.Name} - {b.BankCode}"))}");
        }

        public async Task<IEnumerable<Currency>> GetAccessibleCurrencies(IBank bank)
        {
            if (bank is null)
            {
                _logger?.LogError($"Bank is null.");
                throw new ArgumentNullException(nameof(bank));
            }

            if (!_banks.Contains(bank))
            {
                _logger?.LogError($"Bank [{bank.Name}] does not support.");
                throw new ArgumentException(nameof(bank));
            }

            var result = await bank.GetAccessibleCurrencies();
            if (result is null)
            {
                _logger?.LogWarning($"Bank [{bank.Name}] does not have accessebile currencies.");
                throw new Exception($"Bank [{bank.Name}] does not have accessebile currencies.");
            }

            return result;
        }

        public async Task<IEnumerable<IBank>> GetBanks()
        {
            return _banks;
        }

        public async IAsyncEnumerable<ExchangeRate> GetExchangeRates(ExchangeQuery exchangeQuery, IEnumerable<IBank> banks)
        {
            if (banks is null)
            {
                _logger?.LogError($"Argument banks is null.");
                throw new ArgumentNullException(nameof(banks));
            }

            foreach (var bank in banks)
            {
                if (!_banks.Contains(bank))
                {
                    _logger?.LogError($"{bank} does not support.");
                    throw new ArgumentException(nameof(bank));
                }

                ExchangeRate rate = null;
                try
                {
                    rate = await bank.GetExchangeRate(exchangeQuery);
                }
                catch (Exception ex)
                {
                    _logger?.LogError($"{ex.Message}");
                }

                if (rate is null)
                {
                    _logger?.LogError($"Rate is not available. Create default rate.");
                    rate = new()
                    {
                        BankCode = bank.BankCode
                    };
                }
                _logger?.LogInformation($"Returns rate:\n{rate}.");
                yield return rate;
            }
        }
    }
}
