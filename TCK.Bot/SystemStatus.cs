namespace TCK.Bot
{
    public class DynamicTradesCache
    {
        public List<DynamicOrder[]>? Orders { get; set; } = default!;
        public List<Subscription> Subscriptions { get; set; } = default!;
    }
}
