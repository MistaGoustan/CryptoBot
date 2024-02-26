namespace TCK.Bot.Extensions
{
    public static class QuantityExtensions
    {
        public static decimal ToSize(this decimal quantity, decimal price)
            => quantity * price;

        public static decimal ToSize(this decimal quantity, decimal price, decimal fee)
            => quantity * price - fee;
    }
}
