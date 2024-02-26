using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TCK.Bot.Api
{
    public sealed class SignalTradeRequest
    {
        public Exchange Exchange { get; set; } = default!;

        [DefaultValue("15")]
        [Required(ErrorMessage = "Interval is required")]
        public string Interval { get; set; } = default!;

        public OrderSide OrderSide { get; set; }

        public PositionSide PositionSide { get; set; }

        [DefaultValue(0.1)]
        [Range(0, 1, ErrorMessage = $"{nameof(StopPercent)} must be between 1 - 0")]
        public decimal StopPercent { get; set; }

        [DefaultValue("ETHUSDT")]
        [Required(ErrorMessage = "Ticker is required")]
        public string Ticker { get; set; } = default!;
    }
}
