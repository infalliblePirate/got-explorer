using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
using System.ComponentModel.DataAnnotations;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<ValidationWithEntityModel<UserDTO>> Register(RegisterDTO registerDTO);
        Task<ValidationWithEntityModel<UserDTO>> Login(LoginDTO loginDTO);
    }
}
