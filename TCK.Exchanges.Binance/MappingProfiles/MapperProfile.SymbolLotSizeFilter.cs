using AutoMapper;
using Binance.Net.Objects.Models.Spot;
using TCK.Bot;

namespace TCK.Exchanges.Binance.MappingProfiles
{
    public partial class MapperProfile : Profile
    {
        public void SymbolLotSizeFilterMapperProfile(MapperProfile profile)
        {
            profile.CreateMap<BinanceSymbolLotSizeFilter, SymbolLotSizeFilter>()
                .ForMember(src => src.MaxQuantity, opt => opt.MapFrom(src => src.MaxQuantity))
                .ForMember(src => src.MinQuantity, opt => opt.MapFrom(src => src.MinQuantity))
                .ForMember(src => src.StepSize, opt => opt.MapFrom(src => src.StepSize));
        }
    }
}
