namespace TCK.Bot.DynamicService
{
    public class DynamicOrderCache : IDynamicOrderCache
    {
        private Dictionary<String, DynamicOrder[]> OrderGroups = new();

        public void AddGroup(DynamicOrder[] orders)
        {
            var ticker = orders[0].Ticker;
            var group = OrderGroups.GetValueOrDefault(ticker);

            if (group == null)
            {
                OrderGroups.Add(ticker, orders);
            }
            else
            {
                OrderGroups[ticker] = orders;
            }
        }

        public DynamicOrder[] GetGroup(String ticker) =>
            OrderGroups[ticker];

        public List<DynamicOrder[]>? GetAllGroups()
        {
            List<DynamicOrder[]> orders = new();

            foreach (var og in OrderGroups)
            {
                orders.Add(og.Value);
            }

            return orders.Any() ? orders : null;
        }

        public DynamicOrder[]? GetGroupOrDefault(String ticker) =>
            OrderGroups.GetValueOrDefault(ticker);

        public void RemoveGroup(String ticker) =>
            OrderGroups.Remove(ticker);
    }
}