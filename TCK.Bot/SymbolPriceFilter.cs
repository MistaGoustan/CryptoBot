namespace TCK.Bot
{
    public sealed class SymbolPriceFilter
    {
        public Decimal MaxPrice { get; set; }
        public Decimal MinPrice { get; set; }
        public Decimal TickSize { get; set; }
    }
}
