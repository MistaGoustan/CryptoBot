namespace TCK.Bot.Extensions
{
    public static class TickerExtensions
    {
        public static String ToTickerRight(this String ticker)
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

        public static String ToTickerLeft(this String ticker)
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

        public static Boolean IsStableCoin(this String ticker) =>
            ticker.Equals("USDT")
            || ticker.Equals("BUSD")
            || ticker.Equals("USDC");

        private static String GetLeftSide(String ticker, String tickerBase)
        {
            var part = ticker.Split(tickerBase);

            return part[0] == String.Empty ? tickerBase : part[0];
        }

        private static String GetRightSide(String ticker, String tickerBase)
        {
            var part = ticker.Split(tickerBase);

            return part[1] == String.Empty ? tickerBase : part[1];
        }
    }
}
