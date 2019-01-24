using AutoMapper;

namespace Katil.Business.Services.Mapping
{
    public static class MappingProfile
    {
        public static void Init()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new UserMapping());
                cfg.AddProfile(new FilesMapping());
            });
        }
    }
}