namespace TCK.Bot
{
    public sealed class MiniDynamicOrder
    {
        public decimal BuyPrice { get; set; }
        public decimal Fee { get; set; }
        public decimal PNL { get; set; }
        public PositionSide PositionSide { get; set; }
        public decimal QuantityFilled { get; set; }
        public decimal QuantityQuoted { get; set; }
        public decimal SellPrice { get; set; }
        public DynamicOrderStatus Status { get; set; }
        public decimal StopPrice { get; set; }
        public decimal TargetPrice { get; set; }
        public string Ticker { get; set; } = default!;
    }
}
