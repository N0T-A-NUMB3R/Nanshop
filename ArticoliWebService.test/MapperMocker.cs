using ArticoliWebService.Profiles;
using AutoMapper;

namespace ArticoliWebService.Test
{
    public static class MapperMocker
    {
        public static IMapper GetMapper()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArticoliProfile());
            });

            var mapper = mockMapper.CreateMapper();

            return mapper;
        }
    }
}