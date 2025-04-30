using System.Text;

namespace AB.TelegramBot.Bank.Models
{
    public class ExchangeQuery
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public Currency CurrencyFrom { get; set; } = new();
        public Currency CurrencyTo { get; set; } = new();

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append($"Date: {Date}\n");
            sb.Append($"Currency from: {CurrencyFrom}\n");
            sb.Append($"Currency to: {CurrencyTo}");

            return sb.ToString();
        }

        public bool IsValid()
        {
            return CurrencyFrom is not null &&
                   CurrencyTo is not null &&
                   CurrencyFrom.IsValid() &&
                   CurrencyTo.IsValid();
        }
    }
}
