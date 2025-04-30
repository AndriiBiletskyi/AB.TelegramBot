namespace AB.TelegramBot.Bank.Models
{
    public static class CurrencyNames
    {
        public static readonly Dictionary<string, string> DefaultCurrencyNames = new()
        {
            { "USD", "долар США" },
            { "EUR", "євро" },
            { "CHF", "швейцарський франк" },
            { "GBP", "британський фунт" },
            { "PLN", "польський злотий" },
            { "SEK", "шведська крона" },
            { "XAU", "золото" },
            { "CAD", "канадський долар" },
            { "UAH", "українська гривня" }
        };
    }
}
