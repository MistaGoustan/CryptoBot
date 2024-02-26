namespace TCK.Bot
{
    public sealed class SymbolPercentPriceFilter
    {
        public int AveragePriceMinutes { get; set; }
        public decimal MultiplierDown { get; set; }
        public decimal MultiplierUp { get; set; }
    }
}
