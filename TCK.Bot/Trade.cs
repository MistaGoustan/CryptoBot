namespace TCK.Bot
{
    public class Trade
    {
        public Decimal Fee { get; set; }
        public String OrderId { get; set; } = default!;
        public Decimal Price { get; set; }
        public Decimal Quantity { get; set; }
        public OrderSide Side { get; set; }
    }
}
