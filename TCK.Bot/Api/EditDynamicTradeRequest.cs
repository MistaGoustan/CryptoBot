using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TCK.Bot.Api
{
    public class EditDynamicTradeRequest
    {
        [DefaultValue(1400)]
        [Range(0.00000001, Double.MaxValue, ErrorMessage = $"{nameof(StopPrice)} must be at least 0.00000001")]
        public Decimal StopPrice { get; set; }

        public Exchange Exchange { get; set; } = default!;

        [DefaultValue("ETHUSDT")]
        [Required(ErrorMessage = "Ticker is required")]
        public String Ticker { get; set; } = default!;
    }
}
