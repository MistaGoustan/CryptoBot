using AutoMapper;
using Binance.Net.Objects.Models.Spot;
using TCK.Bot;

namespace TCK.Exchanges.Binance.MappingProfiles
{
    public partial class MapperProfile : Profile
    {
        public void SymbolPriceFilterMapperProfile(MapperProfile profile)
        {
            profile.CreateMap<BinanceSymbolPriceFilter, SymbolPriceFilter>()
                .ForMember(src => src.MaxPrice, opt => opt.MapFrom(src => src.MaxPrice))
                .ForMember(src => src.MinPrice, opt => opt.MapFrom(src => src.MinPrice))
                .ForMember(src => src.TickSize, opt => opt.MapFrom(src => src.TickSize));
        }
    }
}
