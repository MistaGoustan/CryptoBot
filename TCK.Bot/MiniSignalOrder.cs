namespace TCK.Bot
{
    public sealed class MiniSignalOrder
    {
        public Decimal BuyPrice { get; set; }
        public Decimal Fee { get; set; }
        public Decimal? PNL { get; set; }
        public PositionSide PositionSide { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal SellPrice { get; set; }
        public SignalOrderStatus Status { get; set; } = default!;
        public String Ticker { get; set; } = default!;
    }
}
