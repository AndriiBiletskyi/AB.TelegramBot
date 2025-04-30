using AB.TelegramBot.Bank.Abstraction.Interfaces;
using AB.TelegramBot.Bank.Models;
using AB.TelegramBot.DateTimes;
using AB.TelegramBot.Resources.Strings;
using AB.TelegramBot.Web.Clients;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AB.TelegramBot.Banks.PrivatBank
{
    public class PBBank : IBank
    {
        private ILogger<PBBank>? _logger;
        private readonly IWebClient _client;
        private readonly IDateTimeProvider _dateTimeProvider;

        private List<Currency> _currencies = new()
        {
            new Currency() { CurrencyCode = "USD", Name = "долар США" },
            new Currency() { CurrencyCode = "EUR", Name = "євро" },
            new Currency() { CurrencyCode = "CHF", Name = "швейцарський франк" },
            new Currency() { CurrencyCode = "GBP", Name = "британський фунт" },
            new Currency() { CurrencyCode = "PLN", Name = "польський злотий" },
            new Currency() { CurrencyCode = "SEK", Name = "шведська крона" },
            new Currency() { CurrencyCode = "XAU", Name = "золото" },
            new Currency() { CurrencyCode = "CAD", Name = "канадський долар" },
            new Currency() { CurrencyCode = "UAH", Name = "українська гривня" }
        };

        public string Name => "PrivatBank";
        public string BankCode => "PB";
        public string Url => "https://api.privatbank.ua/p24api/";

        public PBBank(IWebClient client,
                      IDateTimeProvider dateTimeProvider,
                      ILoggerFactory? logger = null)
        {
            _logger = logger?.CreateLogger<PBBank>();
            _logger?.LogInformation("Create an instance of PrivatBank.");
            _client = client;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ExchangeRate> GetExchangeRate(ExchangeQuery exchangeQuery)
        {
            if (exchangeQuery is null || !exchangeQuery.IsValid())
            {
                _logger?.LogError("ExchangeQuery is null.");
                throw new ArgumentNullException(nameof(exchangeQuery));
            }

            if (!_currencies.Select(c => c.CurrencyCode.ToUpper()).Contains(exchangeQuery.CurrencyFrom.CurrencyCode.ToUpper()))
            {
                _logger?.LogError($"ExchangeQuery.CurrencyFrom.CurrencyCode - {exchangeQuery.CurrencyFrom.CurrencyCode} is not correct.");
                throw new ArgumentException($"{exchangeQuery.CurrencyFrom.CurrencyCode} {StringResources.IsNotCorrect}");
            }

            if (exchangeQuery.Date.Date > _dateTimeProvider.GetCurrentTime().Date ||
                (exchangeQuery.Date.Date == _dateTimeProvider.GetCurrentTime().Date && _dateTimeProvider.GetCurrentTime().Hour < 9))
            {
                _logger?.LogError("Trying to get data for next day.");
                throw new ArgumentOutOfRangeException(nameof(exchangeQuery.Date));
            }

            _logger?.LogInformation($"Exchange query:\n{exchangeQuery}");

            var result = await _client.GetAsync<PrivatBankExchageTransaction>(Url, BuildRequest(exchangeQuery.Date));

            if (result is not null)
            {
                var rates = ToExchangeRates(result);
                if (rates is not null && rates.Any())
                {
                    var resultRate = rates.FirstOrDefault(r => r.CurrencyFrom == exchangeQuery.CurrencyFrom);
                    if (resultRate is null || !resultRate.IsValid())
                    {
                        _logger?.LogWarning($"Rate for {exchangeQuery} does not exist.");
                        resultRate = new()
                        {
                            BankCode = BankCode,
                            CurrencyFrom = exchangeQuery.CurrencyFrom,
                            CurrencyTo = exchangeQuery.CurrencyTo,
                            Date = exchangeQuery.Date
                        };
                    }
                    return resultRate;
                }
            }

            _logger?.LogError($"{BankCode} returns wrong data.");
            throw new Exception($"{Name} cannot get a rate...");
        }

        public async Task<IEnumerable<Currency>> GetAccessibleCurrencies()
        {
            return _currencies;
        }

        private string BuildRequest(DateTime date)
        {
            _logger?.LogInformation($"PrivatBank.BuildRequest: Starts build a request from date {date}.");
            StringBuilder sb = new();
            sb.Append("exchange_rates?json&date=");
            sb.Append(date.Date.ToString("dd.MM.yyyy"));
            _logger?.LogInformation($"PrivatBank.BuildRequest: Built request is [{sb}].");
            return sb.ToString();
        }

        private IEnumerable<ExchangeRate> ToExchangeRates(PrivatBankExchageTransaction privatBankRates)
        {
            var list = new List<ExchangeRate>();
            foreach (var item in privatBankRates.exchangeRate)
            {
                var rate = new ExchangeRate()
                {
                    Date = ConvertStringToDate(privatBankRates.date),
                    DateOfQuery = _dateTimeProvider.GetCurrentTime().Date,
                    BankCode = privatBankRates.bank.ToUpper(),
                    PurchaseRate = item.purchaseRate,
                    SaleRate = item.saleRate,
                    CurrencyFrom = new Currency() { Name = privatBankRates.CurrencyToName, CurrencyCode = item.currency.ToUpper() },
                    CurrencyTo = new Currency() { CurrencyCode = item.baseCurrency.ToUpper() }
                };
                list.Add(rate);
            }
            return list;
        }

        private DateTime ConvertStringToDate(string value)
        {
            if (DateTime.TryParse(value, out var date))
            {
                return date.Date;
            }

            throw new ArgumentException($"{value} does not contain a date");
        }
    }
}
