namespace TCK.Bot
{
    public class Trade
    {
        public decimal Fee { get; set; }
        public string OrderId { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public OrderSide Side { get; set; }
    }
}
