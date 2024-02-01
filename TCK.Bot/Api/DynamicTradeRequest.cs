using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TCK.Bot.Api
{
    public class DynamicTradeRequest
    {
        [DefaultValue(450)]
        public Decimal AccountBalance { get; set; }

        [DefaultValue(false)]
        public Boolean IsWeighted { get; set; }

        [DefaultValue(10)]
        [Range(2, Int32.MaxValue, ErrorMessage = $"{nameof(NumberOfOrders)} must be at least 2")]
        public Int16 NumberOfOrders { get; set; }

        [DefaultValue(2000)]
        [Range(0.00000001, Double.MaxValue, ErrorMessage = $"{nameof(UpperTargetPrice)} must be at least 0.00000001")]
        public Decimal UpperTargetPrice { get; set; }

        [DefaultValue(1900)]
        [Range(0.00000001, Double.MaxValue, ErrorMessage = $"{nameof(LowerTargetPrice)} must be at least 0.00000001")]
        public Decimal LowerTargetPrice { get; set; }

        [DefaultValue(1880)]
        [Range(0.00000001, Double.MaxValue, ErrorMessage = $"{nameof(UpperBuyPrice)} must be at least 0.00000001")]
        public Decimal UpperBuyPrice { get; set; }

        [DefaultValue(1780)]
        [Range(0.00000001, Double.MaxValue, ErrorMessage = $"{nameof(NumberOfOrders)} must be at least 0.00000001")]
        public Decimal LowerBuyPrice { get; set; }

        [DefaultValue(1699)]
        [Range(0.00000001, Double.MaxValue, ErrorMessage = $"{nameof(StopPrice)} must be at least 0.00000001")]
        public Decimal StopPrice { get; set; }

        public Exchange Exchange { get; set; } = default!;

        [DefaultValue("ETHUSDT")]
        [Required(ErrorMessage = "Ticker is required")]
        public String Ticker { get; set; } = default!;

        public PositionSide PositionSide { get; set; }
    }
}
