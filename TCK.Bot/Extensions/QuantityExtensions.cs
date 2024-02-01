namespace TCK.Bot.Extensions
{
    public static class QuantityExtensions
    {
        public static Decimal ToSize(this Decimal quantity, Decimal price)
            => quantity * price;

        public static Decimal ToSize(this Decimal quantity, Decimal price, Decimal fee)
            => quantity * price - fee;
    }
}
