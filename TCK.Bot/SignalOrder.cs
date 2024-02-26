namespace TCK.Bot
{
    public sealed class SignalOrder
    {
        public int Id { get; set; }
        public DateTime BuyDate { get; set; }
        public decimal BuyFee { get; set; }
        public string BuyOrderId { get; set; } = default!;
        public decimal BuyPrice { get; set; }
        public Exchange Exchange { get; set; }
        public decimal FundingFee { get; set; }
        public string Interval { get; set; } = default!;
        public decimal? PNL { get; set; }
        public PositionSide PositionSide { get; set; }
        public decimal Quantity { get; set; }
        public DateTime? SellDate { get; set; }
        public decimal SellFee { get; set; }
        public string SellOrderId { get; set; } = default!;
        public decimal SellPrice { get; set; }
        public SignalOrderStatus Status { get; set; } = default!;
        public decimal StopPrice { get; set; }
        public string Ticker { get; set; } = default!;
    }
}
