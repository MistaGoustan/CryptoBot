using AutoMapper;
using TCK.Exchanges.Binance.MappingProfiles;
using Xunit;

namespace TCK.Exchanges.Binance.Test
{
    public sealed class MappingProfileTests
    {
        private readonly IMapper _subject;

        public MappingProfileTests()
        {
            _subject = new MapperConfiguration(cfg => { cfg.AddProfile<MapperProfile>(); }).CreateMapper();
        }

        [Fact]
        public void ShouldSetupMappingsCorrectly() =>
            _subject.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
