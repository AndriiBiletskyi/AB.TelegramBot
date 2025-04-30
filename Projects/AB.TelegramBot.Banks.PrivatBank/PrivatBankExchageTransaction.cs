namespace AB.TelegramBot.Banks.PrivatBank
{
    public class PrivatBankExchageTransaction
    {
        public string date { get; set; }
        public string bank { get; set; }
        public int baseCurrency { get; set; }
        public string CurrencyToName { get; set; } = "українська гривня";
        public string baseCurrencyLit { get; set; }
        public PrivatBankExchangeRate[] exchangeRate { get; set; } = new PrivatBankExchangeRate[0];
    }

    public class PrivatBankExchangeRate
    {
        public string baseCurrency { get; set; }
        public string currency { get; set; }
        public float saleRateNB { get; set; }
        public float purchaseRateNB { get; set; }
        public float saleRate { get; set; }
        public float purchaseRate { get; set; }
    }
}
