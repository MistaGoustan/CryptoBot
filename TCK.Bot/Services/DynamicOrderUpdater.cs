using TCK.Bot.Data;

namespace TCK.Bot.Services
{
    public class DynamicOrderUpdater : IDynamicOrderUpdater
    {
        private readonly IDynamicOrderRepository _repository;

        public DynamicOrderUpdater(IDynamicOrderRepository repository)
        {
            _repository = repository;
        }

        public Task<DynamicOrder> UpdateOrderAsync(DynamicOrder order)
        {
            return _repository.UpdateOrderAsync(order);
        }

        public Task<DynamicOrder[]> UpdateOrdersAsync(DynamicOrder[] orders)
        {
            return _repository.UpdateOrdersAsync(orders);
        }
    }
}
