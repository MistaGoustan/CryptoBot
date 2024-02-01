namespace TCK.Bot.Options
{
    public sealed class BinanceOptions
    {
        public String ApiKey { get; set; } = default!;
        public Boolean CheckBalance { get; set; } = default!;
        public String Secret { get; set; } = default!;
    }
}