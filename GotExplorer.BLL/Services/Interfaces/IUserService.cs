using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
using System.ComponentModel.DataAnnotations;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<UserDTO>> RegisterAsync(RegisterDTO registerDTO);
        Task<ServiceResult<UserDTO>> LoginAsync(LoginDTO loginDTO);
        Task<ServiceResult> UpdateUserByIdAsync(UpdateUserDTO updateUserDTO);
        Task<ServiceResult> DeleteUserByIdAsync(string userId);
        Task<ServiceResult> UpdatePasswordAsync(UpdateUserPasswordDTO updateUserPasswordDTO);
        Task<ServiceResult> GeneratePasswordResetLinkAsync(string email, string origin);
        Task<ServiceResult> ResetPasswordAsync(ResetPasswordDTO ResetPasswordDTO);
    }
}
