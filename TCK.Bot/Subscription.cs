namespace TCK.Bot
{
    public class Subscription
    {
        public Int32 Id { get; set; }
        public Decimal LastPrice { get; set; }
        public String Ticker { get; set; } = default!;
    }
}
