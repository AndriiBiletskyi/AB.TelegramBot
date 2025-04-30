using AB.TelegramBot.Bank.Abstraction.Interfaces;
using AB.TelegramBot.Bank.Models;
using AB.TelegramBot.DateTimes;
using AB.TelegramBot.Resources.Strings;
using AB.TelegramBot.Web.Clients;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AB.TelegramBot.Banks.NBU
{
    public class NBUBank : IBank
    {
        private readonly IWebClient _client;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger? _logger;
        private List<Currency> _currencies = new();

        public string Name => "National Bank of Ukraine";
        public string BankCode => "NBU";
        public string Url => "https://bank.gov.ua/NBUStatService/v1/statdirectory/";

        public NBUBank(IWebClient client,
                       IDateTimeProvider dateTimeProvider,
                       ILoggerFactory? logger = null)
        {
            _client = client;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger?.CreateLogger<NBUBank>();
        }

        public async Task<IEnumerable<Currency>> GetAccessibleCurrencies()
        {
            if (_currencies.Any())
            {
                return _currencies;
            }

            _logger?.LogInformation("Starts looking for accessible currencies.");
            var result = await _client.GetAsync<NBUExchageTransaction[]>(Url, BuildRequest(_dateTimeProvider.GetCurrentTime().AddDays(-1)));

            if (result is not null && result.Any())
            {
                var rates = ToCurrencies(result);

                if (rates is not null && rates.Any())
                {
                    rates.Select(c => string.IsNullOrEmpty(c.Name) ?
                        (CurrencyNames.DefaultCurrencyNames.ContainsKey(c.CurrencyCode) ? CurrencyNames.DefaultCurrencyNames[c.CurrencyCode] : string.Empty) : c.Name);

                    _currencies = new(rates);
                    _logger?.LogInformation($"Found currencies:\n{string.Join('\n', _currencies.Select(c => c.ToString()))}");
                    return _currencies;
                }
            }

            _logger?.LogError("Can't return currencies.");
            throw new Exception($"Error. {Name}, currencies do not exist.");
        }

        public async Task<ExchangeRate> GetExchangeRate(ExchangeQuery exchangeQuery)
        {
            if (_currencies is null || !_currencies.Any())
            {
                _currencies = new(await GetAccessibleCurrencies());
            }

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

            if (exchangeQuery.Date.Date > _dateTimeProvider.GetCurrentTime().Date)
            {
                _logger?.LogError("Trying to get data for next day.");
                throw new ArgumentOutOfRangeException(nameof(exchangeQuery.Date));
            }

            _logger?.LogInformation($"Exchange query:\n{exchangeQuery}");

            var result = await _client.GetAsync<NBUExchageTransaction[]>(Url, BuildRequest(exchangeQuery.Date));

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

        private string BuildRequest(DateTime date)
        {
            _logger?.LogInformation($"NBU.BuildRequest: Starts build a request from date {date}.");
            StringBuilder sb = new();
            sb.Append("exchange?date=");
            sb.Append(date.Date.ToString("yyyyMMdd"));
            sb.Append("&json");
            _logger?.LogInformation($"NBU.BuildRequest: Built request is [{sb}].");
            return sb.ToString();
        }

        private IEnumerable<Currency> ToCurrencies(NBUExchageTransaction[] nBUExchageTransactions)
        {
            var currencies = new List<Currency>();
            foreach (var exchangeRate in nBUExchageTransactions)
            {
                if (exchangeRate.IsValid())
                {
                    currencies.Add(new Currency()
                    {
                        CurrencyCode = exchangeRate.cc.ToUpper(),
                        Name = exchangeRate.txt.ToUpper()
                    });
                }
            }
            return currencies;
        }

        private IEnumerable<ExchangeRate> ToExchangeRates(NBUExchageTransaction[] nBUExchageTransactions)
        {
            var rates = new List<ExchangeRate>();
            foreach (var exchangeRate in nBUExchageTransactions)
            {
                if (exchangeRate.IsValid())
                {
                    rates.Add(new ExchangeRate()
                    {
                        Date = ConvertStringToDate(exchangeRate.exchangedate),
                        DateOfQuery = _dateTimeProvider.GetCurrentTime().Date,
                        BankCode = exchangeRate.BankCode.ToUpper(),
                        PurchaseRate = exchangeRate.rate,
                        SaleRate = exchangeRate.rate,
                        CurrencyFrom = new Currency() { Name = exchangeRate.txt, CurrencyCode = exchangeRate.cc.ToUpper() },
                        CurrencyTo = new Currency() { Name = exchangeRate.CurrencyToName, CurrencyCode = exchangeRate.CurrencyToCode.ToUpper() }
                    });
                }
            }
            return rates;
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
