using AutoMapper;
using Katil.Business.Entities.Models.Setting;
using Katil.Data.Model;

namespace Katil.Business.Services.Mapping
{
    public class SettingMapping : Profile
    {
        public SettingMapping()
        {
            CreateMap<SettingResponse, SystemSettings>();
            CreateMap<SystemSettings, SettingResponse>();
        }
    }
}
