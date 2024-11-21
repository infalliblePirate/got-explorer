using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
using System.ComponentModel.DataAnnotations;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<UserDTO>> Register(RegisterDTO registerDTO);
        Task<ServiceResult<UserDTO>> Login(LoginDTO loginDTO);
        Task UpdateUserById(UpdateUserDTO updateUserDTO);
        Task DeleteUserById(string userId);
        Task UpdatePassword(UpdateUserPasswordDTO updateUserPasswordDTO);
        Task<GetUserDTO> GetUserById(string id);
        Task GeneratePasswordResetLink(GeneratePasswordResetLinkDTO generatePasswordResetLinkDTO, string origin);
        Task ResetPassword(ResetPasswordDTO ResetPasswordDTO);
    }
}
