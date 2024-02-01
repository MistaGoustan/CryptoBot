namespace TCK.Bot
{
    public sealed class DynamicIsolatedWallet
    {
        public Decimal AvailableBalance { get; set; }
        public Decimal Balance { get; set; }
        public String Ticker { get; set; } = default!;
    }
}
