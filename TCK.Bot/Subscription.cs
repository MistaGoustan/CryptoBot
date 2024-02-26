namespace TCK.Bot
{
    public class Subscription
    {
        public int Id { get; set; }
        public decimal LastPrice { get; set; }
        public string Ticker { get; set; } = default!;
    }
}
