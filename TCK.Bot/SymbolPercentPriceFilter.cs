namespace TCK.Bot
{
    public sealed class SymbolPercentPriceFilter
    {
        public Int32 AveragePriceMinutes { get; set; }
        public Decimal MultiplierDown { get; set; }
        public Decimal MultiplierUp { get; set; }
    }
}
