using AutoMapper;
using GotExplorer.BLL.DTOs;
using GotExplorer.DAL.Entities;
namespace GotExplorer.BLL.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<User, RegisterDTO>().ReverseMap();
            CreateMap<User, LoginDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UpdateUserDTO>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImageId, opt => opt.PreCondition((src,dest) => src.ImageId != null))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<User, GetUserDTO>().ReverseMap();
        }
    }
}
