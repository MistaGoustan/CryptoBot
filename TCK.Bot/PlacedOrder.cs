namespace TCK.Bot
{
    public class PlacedOrder
    {
        public decimal Price { get; set; }
        public decimal Fee { get; set; }
        public string OrderId { get; set; } = default!;
        public decimal Quantity { get; set; }
    }
}
