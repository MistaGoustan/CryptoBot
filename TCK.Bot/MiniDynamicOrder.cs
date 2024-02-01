namespace TCK.Bot
{
    public sealed class MiniDynamicOrder
    {
        public Decimal BuyPrice { get; set; }
        public Decimal Fee { get; set; }
        public Decimal PNL { get; set; }
        public PositionSide PositionSide { get; set; }
        public Decimal QuantityFilled { get; set; }
        public Decimal QuantityQuoted { get; set; }
        public Decimal SellPrice { get; set; }
        public DynamicOrderStatus Status { get; set; }
        public Decimal StopPrice { get; set; }
        public Decimal TargetPrice { get; set; }
        public String Ticker { get; set; } = default!;
    }
}
