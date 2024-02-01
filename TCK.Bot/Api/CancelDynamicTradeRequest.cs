using System.ComponentModel.DataAnnotations;

namespace TCK.Bot.Api
{
    public class CancelDynamicTradeRequest
    {
        public Exchange Exchange { get; set; } = default!;

        [Required(ErrorMessage = "Ticker(s) is required")]
        public List<String> Tickers { get; set; } = default!;
    }
}
