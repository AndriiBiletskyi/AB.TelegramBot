namespace AB.TelegramBot.Banks.NBU
{
    public class NBUExchageTransaction
    {
        public string BankCode { get; set; } = "NBU";
        public string CurrencyToCode { get; set; } = "UAH";
        public string CurrencyToName { get; set; } = "українська гривня";
        public int r030 { get; set; }
        //currency name
        public string txt { get; set; }
        public float rate { get; set; }
        //currency code
        public string cc { get; set; }
        public string exchangedate { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(txt) &&
                   !string.IsNullOrEmpty(cc) &&
                   !string.IsNullOrEmpty(exchangedate);
        }

    }
}