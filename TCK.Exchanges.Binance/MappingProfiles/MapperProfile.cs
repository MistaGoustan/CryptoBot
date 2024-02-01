using AutoMapper;

namespace TCK.Exchanges.Binance.MappingProfiles
{
    public partial class MapperProfile : Profile
    {
        public MapperProfile()
        {
            SymbolLotSizeFilterMapperProfile(this);
            SymbolPercentPriceFilterMapperProfile(this);
            SymbolPriceFilterMapperProfile(this);
        }
    }
}
