namespace TCK.Bot
{
    public sealed class DynamicOrder
    {
        public DateTime BuyDate { get; set; }
        public Decimal BuyFee { get; set; }
        public String BuyOrderId { get; set; } = default!;
        public Decimal BuyPrice { get; set; }
        public Exchange Exchange { get; set; }
        public Decimal FundingFee { get; set; }
        public Int64 Id { get; set; }
        public String OrderGroupId { get; set; } = default!;
        public Decimal PNL { get; set; }
        public PositionSide PositionSide { get; set; }
        public Decimal QuantityFilled { get; set; }
        public Decimal QuantityQuoted { get; set; }
        public DateTime? SellDate { get; set; }
        public Decimal SellFee { get; set; }
        public String SellOrderId { get; set; } = default!;
        public Decimal SellPrice { get; set; }
        public DynamicOrderStatus Status { get; set; }
        public Decimal StopPrice { get; set; }
        public Decimal TargetPrice { get; set; }
        public Decimal TargetQuantity { get; set; }
        public String Ticker { get; set; } = default!;
    }
}
