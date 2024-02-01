
namespace TCK.Bot.Services
{
    public interface ITickerValidator
    {
        Task<Boolean> TickerPairExistsAsync(Exchange exchange, String ticker);
    }
}