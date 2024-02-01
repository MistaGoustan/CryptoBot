
namespace TCK.Bot.Services
{
    public interface IDynamicOrderRetriever
    {
        Task<DynamicOrder[]> GetUncompletedOrdersAsync();
    }
}