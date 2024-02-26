namespace TCK.Bot
{
    public sealed class DynamicIsolatedWallet
    {
        public decimal AvailableBalance { get; set; }
        public decimal Balance { get; set; }
        public string Ticker { get; set; } = default!;
    }
}
