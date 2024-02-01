namespace TCK.Bot.Options
{
    public class ConfigurationOptions
    {
        public virtual Decimal BuyFee { get; set; }
        public virtual Decimal DefaultWalletSize { get; set; }
        // TODO: rename to MoveStopPricePercent
        public virtual Decimal DynamicMoveUpStopPricePercent { get; set; }
        public virtual Decimal DynamicRiskPercent { get; set; }
        public virtual Int16 ExpireTimeInSeconds { get; set; }
        public virtual Boolean HasLiveTestPrices { get; set; }
        public virtual Boolean IsProduction { get; set; }
        public virtual Decimal SellFee { get; set; }
        public virtual Decimal SignalRiskPercent { get; set; }
    }
}
