using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TCK.Bot.Api
{
    public class EditDynamicTradeRequest
    {
        [DefaultValue(1400)]
        [Range(0.00000001, Double.MaxValue, ErrorMessage = $"{nameof(StopPrice)} must be at least 0.00000001")]
        public decimal StopPrice { get; set; }

        public Exchange Exchange { get; set; } = default!;

        [DefaultValue("ETHUSDT")]
        [Required(ErrorMessage = "Ticker is required")]
        public string Ticker { get; set; } = default!;
    }
}
