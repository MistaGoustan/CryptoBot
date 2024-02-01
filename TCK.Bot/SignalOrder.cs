namespace TCK.Bot
{
    public sealed class SignalOrder
    {
        public Int32 Id { get; set; }
        public DateTime BuyDate { get; set; }
        public Decimal BuyFee { get; set; }
        public String BuyOrderId { get; set; } = default!;
        public Decimal BuyPrice { get; set; }
        public Exchange Exchange { get; set; }
        public Decimal FundingFee { get; set; }
        public String Interval { get; set; } = default!;
        public Decimal? PNL { get; set; }
        public PositionSide PositionSide { get; set; }
        public Decimal Quantity { get; set; }
        public DateTime? SellDate { get; set; }
        public Decimal SellFee { get; set; }
        public String SellOrderId { get; set; } = default!;
        public Decimal SellPrice { get; set; }
        public SignalOrderStatus Status { get; set; } = default!;
        public Decimal StopPrice { get; set; }
        public String Ticker { get; set; } = default!;
    }
}
