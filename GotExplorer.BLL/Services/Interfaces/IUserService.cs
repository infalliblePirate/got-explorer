using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
using FluentValidation.Results;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<ValidationWithEntityModel<UserDTO>> RegisterAsync(RegisterDTO registerDTO);
        Task<ValidationWithEntityModel<UserDTO>> LoginAsync(LoginDTO loginDTO);
        Task<ValidationResult> UpdateUserByIdAsync(UpdateUserDTO updateUserDTO);
        Task<ValidationResult> DeleteUserByIdAsync(string userId);
        Task<ValidationResult> UpdatePasswordAsync(UpdateUserPasswordDTO updateUserPasswordDTO);
        Task<ValidationResult> GeneratePasswordResetLinkAsync(string email);
        Task<ValidationResult> ResetPasswordAsync(ResetPasswordDTO ResetPasswordDTO);
    }
}
