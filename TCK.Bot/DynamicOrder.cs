namespace TCK.Bot
{
    public sealed class DynamicOrder
    {
        public DateTime BuyDate { get; set; }
        public decimal BuyFee { get; set; }
        public string BuyOrderId { get; set; } = default!;
        public decimal BuyPrice { get; set; }
        public Exchange Exchange { get; set; }
        public decimal FundingFee { get; set; }
        public int Id { get; set; }
        public string OrderGroupId { get; set; } = default!;
        public decimal PNL { get; set; }
        public PositionSide PositionSide { get; set; }
        public decimal QuantityFilled { get; set; }
        public decimal QuantityQuoted { get; set; }
        public DateTime? SellDate { get; set; }
        public decimal SellFee { get; set; }
        public string SellOrderId { get; set; } = default!;
        public decimal SellPrice { get; set; }
        public DynamicOrderStatus Status { get; set; }
        public decimal StopPrice { get; set; }
        public decimal TargetPrice { get; set; }
        public decimal TargetQuantity { get; set; }
        public string Ticker { get; set; } = default!;
    }
}
