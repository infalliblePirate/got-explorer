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
                .ForMember(dest => dest.UserName, opt =>
                { 
                    opt.MapFrom(src => src.Username);
                    opt.PreCondition(src => src.Username != null);
                })
                .ForMember(dest => dest.Email, opt =>
                {
                    opt.MapFrom(src => src.Email);
                    opt.PreCondition(src => src.Email != null); 
                })
                .ForMember(dest => dest.ImageId, opt =>
                {
                    opt.MapFrom(src => src.ImageId);
                    opt.PreCondition(src => src.ImageId != null);
                });
        }
    }
}
