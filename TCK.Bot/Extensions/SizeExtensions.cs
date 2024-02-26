namespace TCK.Bot.Extensions
{
    public static class SizeExtensions
    {
        public static decimal ToStepSize(this decimal size, SymbolLotSizeFilter lotSize)
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

            return decimal.Round(size, 8);
        }
    }
}
