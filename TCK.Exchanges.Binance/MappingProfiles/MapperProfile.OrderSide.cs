using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using BinanceEnums = Binance.Net.Enums;

namespace TCK.Exchanges.Binance.MappingProfiles
{
    // Enum mapping docs https://docs.automapper.org/en/stable/Enum-Mapping.html
    public partial class MapperProfile : Profile
    {
        public void OrderSideMapperProfile(MapperProfile profile)
        {
            profile.CreateMap<BinanceEnums.OrderSide, TCK.Bot.OrderSide>()
                .ConvertUsingEnumMapping(opt => opt.MapByValue());
        }
    }
}
