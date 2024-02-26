namespace TCK.Bot.Options
{
    public class ConfigurationOptions
    {
        public virtual decimal BuyFee { get; set; }
        public virtual decimal DefaultWalletSize { get; set; }
        // TODO: rename to MoveStopPricePercent
        public virtual decimal DynamicMoveUpStopPricePercent { get; set; }
        public virtual decimal DynamicRiskPercent { get; set; }
        public virtual short ExpireTimeInSeconds { get; set; }
        public virtual bool HasLiveTestPrices { get; set; }
        public virtual bool IsProduction { get; set; }
        public virtual decimal SellFee { get; set; }
        public virtual decimal SignalRiskPercent { get; set; }
    }
}
