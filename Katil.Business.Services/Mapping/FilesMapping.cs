using AutoMapper;
using Katil.Business.Entities.Models.Files;
using Katil.Common.Utilities;
using Katil.Data.Model;

namespace Katil.Business.Services.Mapping
{
    public class FilesMapping : Profile
    {
        public FilesMapping()
        {
            CreateMap<FileRequest, File>();

            CreateMap<File, FileResponse>()
                .ForMember(x => x.FileDate, opt => opt.MapFrom(src => src.FileDate.ToCmDateTimeString()))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToCmDateTimeString()))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate.ToCmDateTimeString()));
            CreateMap<File, FileRequest>();
        }
    }
}
