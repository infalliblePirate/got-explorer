using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
using System.ComponentModel.DataAnnotations;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<UserDTO>> Register(RegisterDTO registerDTO);
        Task<ServiceResult<UserDTO>> Login(LoginDTO loginDTO);
        Task UpdateUserByIdAsync(UpdateUserDTO updateUserDTO);
        Task DeleteUserByIdAsync(string userId);
        Task UpdatePasswordAsync(UpdateUserPasswordDTO updateUserPasswordDTO);
        Task GeneratePasswordResetLinkAsync(string email, string origin);
        Task ResetPasswordAsync(ResetPasswordDTO ResetPasswordDTO);
    }
}
