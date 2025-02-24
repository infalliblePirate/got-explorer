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

            CreateMap<Image, ImageDTO>().ReverseMap();
            CreateMap<Model3D, Model3dDTO>().ReverseMap();
            CreateMap<Level, LevelDTO>().ReverseMap();
            CreateMap<Level, CreateLevelDTO>().ReverseMap()
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.Models, opt =>
                 {
                    opt.MapFrom(src => src.Models);
                    opt.PreCondition(src => src.Models != null);
                 });
            CreateMap<Level, UpdateLevelDTO>().ReverseMap()
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.Name, opt =>
                 {
                     opt.MapFrom(src => src.Name);
                     opt.PreCondition(src => src.Name != null);
                 })
                .ForMember(dest => dest.X, opt =>
                {
                    opt.MapFrom(src => src.X);
                    opt.PreCondition(src => src.X != null);
                })
                .ForMember(dest => dest.Y, opt =>
                {
                    opt.MapFrom(src => src.Y);
                    opt.PreCondition(src => src.Y != null);
                })
                .ForMember(dest => dest.Models, opt => 
                {
                    opt.MapFrom(src => src.Models);
                    opt.PreCondition(src => src.Models != null);
                    //opt.Ignore();
                });
            CreateMap<Model3D, int>()
                .ConstructUsing(src => src.Id);

            CreateMap<int, Model3D>()
                .ConstructUsing(src => new Model3D { Id = src});
                CreateMap<GameLevel, UpdateGameLevelDTO>()
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.GameId))
                .ForMember(dest => dest.LevelId, opt => opt.MapFrom(src => src.LevelId))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score));

            CreateMap<Game, GameResultDTO>()
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
