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
        }
    }
}
