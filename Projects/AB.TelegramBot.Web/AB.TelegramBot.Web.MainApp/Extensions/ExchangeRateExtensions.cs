using AB.TelegramBot.Bank.Models;
using AB.TelegramBot.Resources.Strings;
using System.Text;

namespace AB.TelegramBot.Web.MainApp.Extensions
{
    public static class ExchangeRateExtensions
    {
        public static string ToTelegramBotString(this ExchangeRate exchangeRate)
        {
            StringBuilder sb = new();

            sb.Append($"{exchangeRate.BankCode ?? string.Empty} ");
            sb.Append($"({exchangeRate.CurrencyTo.CurrencyCode ?? string.Empty}): ");
            sb.Append($"{exchangeRate.CurrencyFrom.CurrencyCode ?? string.Empty} ");

            if (exchangeRate.PurchaseRate is null &&
                exchangeRate.SaleRate is null)
            {
                sb.Append(StringResources.IsNotAvailable);
            }

            if (exchangeRate.SaleRate is not null)
            {
                sb.Append(Convert.ToString(exchangeRate.SaleRate));
            }

            if (exchangeRate.PurchaseRate is not null)
            {
                sb.Append("/");
                sb.Append(Convert.ToString(exchangeRate.PurchaseRate));
            }

            sb.Append($" {exchangeRate.Date.ToString("dd.MM.yyyy")}");

            return sb.ToString();
        }
    }
}
