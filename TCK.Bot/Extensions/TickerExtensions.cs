namespace TCK.Bot.Extensions
{
    public static class TickerExtensions
    {
        public static string ToTickerRight(this string ticker)
        {
            if (ticker.Contains("USDT"))
            {
                return GetRightSide(ticker, "USDT");
            }
            else if (ticker.Contains("USDC"))
            {
                return GetRightSide(ticker, "USDC");
            }
            else if (ticker.Contains("BUSD"))
            {
                return GetRightSide(ticker, "BUSD");
            }
            else if (ticker.Contains("USD"))
            {
                return GetLeftSide(ticker, "USD");
            }

            throw new Exception("Ticker does not contain valid tickerbase.");
        }

        public static string ToTickerLeft(this string ticker)
        {
            if (ticker.Contains("USDT"))
            {
                return GetLeftSide(ticker, "USDT");
            }
            else if (ticker.Contains("USDC"))
            {
                return GetLeftSide(ticker, "USDC");
            }
            else if (ticker.Contains("BUSD"))
            {
                return GetLeftSide(ticker, "BUSD");
            }
            else if (ticker.Contains("USD"))
            {
                return GetLeftSide(ticker, "USD");
            }

            throw new Exception("Ticker does not contain valid tickerbase.");
        }

        public static bool IsStableCoin(this string ticker) =>
            ticker.Equals("USDT")
            || ticker.Equals("BUSD")
            || ticker.Equals("USDC");

        private static string GetLeftSide(string ticker, string tickerBase)
        {
            var part = ticker.Split(tickerBase);

            return part[0] == string.Empty ? tickerBase : part[0];
        }

        private static string GetRightSide(string ticker, string tickerBase)
        {
            var part = ticker.Split(tickerBase);

            return part[1] == string.Empty ? tickerBase : part[1];
        }
    }
}
