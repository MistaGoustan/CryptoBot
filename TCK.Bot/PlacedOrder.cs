namespace TCK.Bot
{
    public class PlacedOrder
    {
        public Decimal Price { get; set; }
        public Decimal Fee { get; set; }
        public String OrderId { get; set; } = default!;
        public Decimal Quantity { get; set; }
    }
}
