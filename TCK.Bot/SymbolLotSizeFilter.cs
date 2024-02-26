namespace TCK.Bot
{
    public sealed class SymbolLotSizeFilter
    {
        public decimal MaxQuantity { get; set; }
        public decimal MinQuantity { get; set; }
        public decimal StepSize { get; set; }
    }
}
