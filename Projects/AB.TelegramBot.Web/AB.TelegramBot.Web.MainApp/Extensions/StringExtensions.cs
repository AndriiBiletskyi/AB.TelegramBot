using AB.TelegramBot.Bank.Models;
using AB.TelegramBot.Resources.Strings;

namespace AB.TelegramBot.Web.MainApp.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsExchangeQuery(this string input, out ExchangeQuery exchangeQuery, out string error)
        {
            Currency currencyFrom = new();
            Currency currencyTo = new();
            var date = DateTime.Now;
            exchangeQuery = new();
            error = string.Empty;

            if (string.IsNullOrEmpty(input))
            {
                error = StringResources.InputDataIsNotCorrect;
                return false;
            }

            List<string> items = input.Split(' ').ToList();
            items.RemoveAll(s => string.IsNullOrEmpty(s));
            if (items.Count > 0)
            {
                if (items[0].Contains("/"))
                {
                    int index = items[0].IndexOf('/');
                    currencyFrom.CurrencyCode = items[0].Substring(0, index);
                    currencyTo.CurrencyCode = items[0].Substring(index + 1);
                    if (string.IsNullOrEmpty(currencyFrom.CurrencyCode))
                    {
                        error = StringResources.InputCurrencyIsNotCorrect;
                        return false;
                    }
                }
                else
                {
                    currencyFrom.CurrencyCode = items[0];
                }

                if (items.Count > 1)
                {
                    if (!DateTime.TryParse(items[1], out date))
                    {
                        error = StringResources.InputDateIsNotCorrect;
                        return false;
                    }
                }

                exchangeQuery.Date = date;
                exchangeQuery.CurrencyFrom = currencyFrom;
                exchangeQuery.CurrencyTo = currencyTo;

                return true;
            }
            error = StringResources.InputDataIsNotCorrect;
            return false;
        }
    }
}
