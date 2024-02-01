using TCK.Bot.Data;

namespace TCK.Bot.Services
{
    internal class DynamicOrderRetriever : IDynamicOrderRetriever
    {
        private readonly IDynamicOrderRepository _repository;

        public DynamicOrderRetriever(IDynamicOrderRepository repository)
        {
            _repository = repository;
        }

        public Task<DynamicOrder[]> GetUncompletedOrdersAsync()
        {
            return _repository.GetUncompletedOrdersAsync();
        }
    }
}
