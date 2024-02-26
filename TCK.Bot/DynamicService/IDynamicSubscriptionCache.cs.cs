namespace TCK.Bot.DynamicService
{
    public interface IDynamicSubscriptionCache
    {
        public void Add(Subscription subscription);
        public List<Subscription> GetAll();
        public Subscription GetOrDefault(string ticker);
        public void Remove(Subscription subscription);
    }
}