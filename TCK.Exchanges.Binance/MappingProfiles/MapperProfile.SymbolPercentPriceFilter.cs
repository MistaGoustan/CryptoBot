using AutoMapper;
using Binance.Net.Objects.Models.Spot;
using TCK.Bot;

namespace TCK.Exchanges.Binance.MappingProfiles
{
    public partial class MapperProfile : Profile
    {
        public void SymbolPercentPriceFilterMapperProfile(MapperProfile profile)
        {
            profile.CreateMap<BinanceSymbolPercentPriceFilter, SymbolPercentPriceFilter>()
                .ForMember(src => src.AveragePriceMinutes, opt => opt.MapFrom(src => src.AveragePriceMinutes))
                .ForMember(src => src.MultiplierDown, opt => opt.MapFrom(src => src.MultiplierDown))
                .ForMember(src => src.MultiplierUp, opt => opt.MapFrom(src => src.MultiplierUp));
        }
    }
}
