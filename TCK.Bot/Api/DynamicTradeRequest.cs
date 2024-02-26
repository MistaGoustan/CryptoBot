using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TCK.Bot.Api
{
    public class DynamicTradeRequest
    {
        [DefaultValue(450)]
        public decimal AccountBalance { get; set; }

        [DefaultValue(false)]
        public bool IsWeighted { get; set; }

        [DefaultValue(10)]
        [Range(2, int.MaxValue, ErrorMessage = $"{nameof(NumberOfOrders)} must be at least 2")]
        public short NumberOfOrders { get; set; }

        [DefaultValue(2000)]
        [Range(0.00000001, Double.MaxValue, ErrorMessage = $"{nameof(UpperTargetPrice)} must be at least 0.00000001")]
        public decimal UpperTargetPrice { get; set; }

        [DefaultValue(1900)]
        [Range(0.00000001, Double.MaxValue, ErrorMessage = $"{nameof(LowerTargetPrice)} must be at least 0.00000001")]
        public decimal LowerTargetPrice { get; set; }

        [DefaultValue(1880)]
        [Range(0.00000001, Double.MaxValue, ErrorMessage = $"{nameof(UpperBuyPrice)} must be at least 0.00000001")]
        public decimal UpperBuyPrice { get; set; }

        [DefaultValue(1780)]
        [Range(0.00000001, Double.MaxValue, ErrorMessage = $"{nameof(NumberOfOrders)} must be at least 0.00000001")]
        public decimal LowerBuyPrice { get; set; }

        [DefaultValue(1699)]
        [Range(0.00000001, Double.MaxValue, ErrorMessage = $"{nameof(StopPrice)} must be at least 0.00000001")]
        public decimal StopPrice { get; set; }

        public Exchange Exchange { get; set; } = default!;

        [DefaultValue("ETHUSDT")]
        [Required(ErrorMessage = "Ticker is required")]
        public string Ticker { get; set; } = default!;

        public PositionSide PositionSide { get; set; }
    }
}
