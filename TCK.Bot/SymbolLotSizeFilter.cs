namespace TCK.Bot
{
    public sealed class SymbolLotSizeFilter
    {
        public Decimal MaxQuantity { get; set; }
        public Decimal MinQuantity { get; set; }
        public Decimal StepSize { get; set; }
    }
}
