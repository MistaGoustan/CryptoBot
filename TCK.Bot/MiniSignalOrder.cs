namespace TCK.Bot
{
    public sealed class MiniSignalOrder
    {
        public decimal BuyPrice { get; set; }
        public decimal Fee { get; set; }
        public decimal? PNL { get; set; }
        public PositionSide PositionSide { get; set; }
        public decimal Quantity { get; set; }
        public decimal SellPrice { get; set; }
        public SignalOrderStatus Status { get; set; } = default!;
        public string Ticker { get; set; } = default!;
    }
}
