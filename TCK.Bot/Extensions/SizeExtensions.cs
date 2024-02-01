namespace TCK.Bot.Extensions
{
    public static class SizeExtensions
    {
        public static Decimal ToStepSize(this Decimal size, SymbolLotSizeFilter lotSize)
        {
            size = Math.Floor(size / lotSize.StepSize) * lotSize.StepSize;

            if (size < lotSize.MinQuantity)
            {
                return lotSize.MinQuantity;
            }

            if (size > lotSize.MaxQuantity)
            {
                return lotSize.MaxQuantity;
            }

            return Decimal.Round(size, 8);
        }
    }
}
