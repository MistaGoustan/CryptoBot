
namespace TCK.Bot.Services
{
    public interface ITickerValidator
    {
        Task<bool> TickerPairExistsAsync(Exchange exchange, string ticker);
    }
}