using AutoMapper;
using Katil.Business.Entities.Models.User;
using Katil.Common.Utilities;
using Katil.Data.Model;

namespace Katil.Business.Services.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserResponse>()
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToCmDateTimeString()))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate.ToCmDateTimeString()));

            CreateMap<User, PatchUserRequest>();

            // Resource to domain
            CreateMap<PatchUserRequest, User>();

            CreateMap<UserLoginRequest, User>();
            CreateMap<User, UserLoginResponse>()
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToCmDateTimeString()))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate.ToCmDateTimeString()));

            CreateMap<UserLoginPatchRequest, User>();
            CreateMap<UserLoginResetRequest, User>();
        }
    }
}