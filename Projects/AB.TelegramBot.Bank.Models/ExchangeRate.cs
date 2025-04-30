using System.Text;

namespace AB.TelegramBot.Bank.Models
{
    public class ExchangeRate
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public DateTime DateOfQuery { get; set; } = DateTime.Now;
        public string BankCode { get; set; } = string.Empty;
        public Currency CurrencyFrom { get; set; } = new();
        public Currency CurrencyTo { get; set; } = new();
        public float? SaleRate { get; set; } = null;
        public float? PurchaseRate { get; set; } = null;

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append($"Date - {Date}\n");
            sb.Append($"Current Date - {DateOfQuery}\n");
            sb.Append($"Bank code - {BankCode}\n");
            sb.Append($"Currency from - {CurrencyFrom}\n");
            sb.Append($"Currency to - {CurrencyTo}\n");
            sb.Append($"Sale rate - {SaleRate}\n");
            sb.Append($"Purchase rate - {PurchaseRate}");
            return sb.ToString();
        }

        public override bool Equals(object? obj)
        {
            return obj is ExchangeRate rate &&
                Date == rate.Date &&
                DateOfQuery == rate.DateOfQuery &&
                BankCode == rate.BankCode &&
                CurrencyFrom == rate.CurrencyFrom &&
                CurrencyTo == rate.CurrencyTo &&
                SaleRate == rate.SaleRate &&
                PurchaseRate == rate.PurchaseRate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BankCode, CurrencyFrom, CurrencyTo, Date);
        }

        public static bool operator ==(ExchangeRate a, ExchangeRate b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            return a is not null && a.Equals(b);
        }

        public static bool operator !=(ExchangeRate a, ExchangeRate b)
        {
            return !(a == b);
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(BankCode) &&
                   CurrencyFrom.IsValid() &&
                   CurrencyTo.IsValid();
        }
    }
}
