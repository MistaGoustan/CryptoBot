namespace TCK.Bot.Options
{
    public sealed class BinanceOptions
    {
        public string ApiKey { get; set; } = default!;
        public bool CheckBalance { get; set; } = default!;
        public string Secret { get; set; } = default!;
    }
}