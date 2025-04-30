namespace AB.TelegramBot.Bank.Models
{
    public class Currency
    {
        public string Name { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{CurrencyCode ?? string.Empty} - " +
                   $"{Name ?? string.Empty}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Currency currency &&
                CurrencyCode.ToUpper() == currency.CurrencyCode.ToUpper();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, CurrencyCode);
        }

        public static bool operator ==(Currency a, Currency b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            return a is not null && a.Equals(b);
        }

        public static bool operator !=(Currency a, Currency b)
        {
            return !(a == b);
        }

        public bool IsValid()
        {
            return Name is not null &&
                   !string.IsNullOrEmpty(CurrencyCode);
        }
    }
}
