using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Extensions;
using GotExplorer.BLL.Options;
using GotExplorer.BLL.Services.Interfaces;
using GotExplorer.BLL.Services.Results;
using GotExplorer.BLL.Validators;
using GotExplorer.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace GotExplorer.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly FrontendOptions _frontendOptions;
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IValidator<RegisterDTO> _registerDtoValidator;
        private readonly IValidator<LoginDTO> _loginDtoValidator;
        private readonly IValidator<UpdateUserDTO> _updateUserDtoValidator;
        private readonly IValidator<UpdateUserPasswordDTO> _updateUserPasswordDtoValidator;
        private readonly IValidator<ResetPasswordDTO> _resetPasswordDtoValidator;
        public UserService(
            IOptions<FrontendOptions> frontendOptions,
            IJwtService jwtService,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper,
            IValidator<LoginDTO> loginDtoValidator,
            IValidator<RegisterDTO> registerDtoValidator,
            IValidator<UpdateUserDTO> updateUserDtoValidator,
            IValidator<UpdateUserPasswordDTO> updateUserPasswordDtoValidator,
            IValidator<ResetPasswordDTO> resetPasswordDtoValidator,
            IEmailService emailService)
        {
            _frontendOptions = frontendOptions.Value;
            _jwtService = jwtService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailService = emailService;
            _loginDtoValidator = loginDtoValidator;
            _registerDtoValidator = registerDtoValidator;
            _updateUserDtoValidator = updateUserDtoValidator;
            _updateUserPasswordDtoValidator = updateUserPasswordDtoValidator;
            _resetPasswordDtoValidator = resetPasswordDtoValidator;
        }

        public async Task<ValidationWithEntityModel<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var validationResult = await _loginDtoValidator.ValidateAsync(loginDTO);

            if (!validationResult.IsValid)
            {
                return new ValidationWithEntityModel<UserDTO>(validationResult);
            }

            var user = await _userManager.FindByNameAsync(loginDTO.Username) ?? await _userManager.FindByEmailAsync(loginDTO.Username);

            if (user == null)
            {
                return new ValidationWithEntityModel<UserDTO>(
                    new ValidationFailure(nameof(loginDTO.Username), ErrorMessages.UserServiceIncorrectUsername, loginDTO.Username) { ErrorCode = ErrorCodes.Unauthorized }
                );
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded)
            {
                return new ValidationWithEntityModel<UserDTO>(
                    new ValidationFailure(nameof(loginDTO.Password), ErrorMessages.UserServiceIncorrectPassword, loginDTO.Password) { ErrorCode = ErrorCodes.Unauthorized }
                );
            }

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Token = _jwtService.GenerateToken(user);
            return new ValidationWithEntityModel<UserDTO>(userDto);
        }

        public async Task<ValidationWithEntityModel<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var validationResult = await _registerDtoValidator.ValidateAsync(registerDTO);

            if (!validationResult.IsValid)
            {
                return new ValidationWithEntityModel<UserDTO>(validationResult);
            }

            var user = _mapper.Map<User>(registerDTO);

            var createdUser = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!createdUser.Succeeded)
            {
                return new ValidationWithEntityModel<UserDTO>(createdUser.ToValidationResult(ErrorCodes.UserCreationFailed));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                return new ValidationWithEntityModel<UserDTO>(roleResult.ToValidationResult(ErrorCodes.RoleAssignmentFailed));
            }

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Token = _jwtService.GenerateToken(user);
            return new ValidationWithEntityModel<UserDTO>(userDto);
        }

        public async Task<ValidationResult> UpdateUserByIdAsync(UpdateUserDTO updateUserDTO)
        {
            var validationResult = await _updateUserDtoValidator.ValidateAsync(updateUserDTO);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            var user = await _userManager.FindByIdAsync(updateUserDTO.Id);

            if (user == null)
            {
                return new ValidationResult(
                [
                    new ValidationFailure(nameof(updateUserDTO.Id), ErrorMessages.UserServiceUserNotFound, updateUserDTO.Id) { ErrorCode=ErrorCodes.NotFound },
                ]);

            }

            _mapper.Map(updateUserDTO, user);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return result.ToValidationResult(ErrorCodes.UserUpdateFailed);
            }

            return new ValidationResult();
        }

        public async Task<ValidationResult> UpdatePasswordAsync(UpdateUserPasswordDTO updateUserPasswordDTO)
        {
            var validationResult = await _updateUserPasswordDtoValidator.ValidateAsync(updateUserPasswordDTO);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            var user = await _userManager.FindByIdAsync(updateUserPasswordDTO.Id);

            if (user == null)
            {
                return new ValidationResult(
                    [
                        new ValidationFailure(nameof(updateUserPasswordDTO.Id), ErrorMessages.UserServiceUserNotFound, updateUserPasswordDTO.Id) { ErrorCode = ErrorCodes.NotFound }
                    ]);
            }

            var result = await _userManager.ChangePasswordAsync(user, updateUserPasswordDTO.CurrentPassword, updateUserPasswordDTO.NewPassword);

            if (!result.Succeeded)
            {
                return result.ToValidationResult(ErrorCodes.UserPasswordUpdateFailed);
            }
            return new ValidationResult();
        }

        public async Task<ValidationResult> GeneratePasswordResetLinkAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new ValidationResult(
                    [
                        new ValidationFailure(nameof(email), ErrorMessages.UserServiceUserNotFound, email) { ErrorCode = ErrorCodes.NotFound },
                    ]);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = $"{_frontendOptions.BaseURL}{_frontendOptions.ResetPasswordPath}?id={user.Id}&token={token}";
            await _emailService.SendEmailAsync(email, "GOTExplorer - Forgot your password?", url);

            return new ValidationResult();
        }

        public async Task<ValidationResult> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var validationResult = await _resetPasswordDtoValidator.ValidateAsync(resetPasswordDTO);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            var user = await _userManager.FindByIdAsync(resetPasswordDTO.Id.ToString());
            if (user == null)
            {
                return new ValidationResult(
                    [
                        new ValidationFailure(nameof(resetPasswordDTO.Id), ErrorMessages.UserServiceUserNotFound,resetPasswordDTO.Id) { ErrorCode = ErrorCodes.NotFound },
                    ]);

            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password);

            if (!result.Succeeded)
            {
                return result.ToValidationResult(ErrorCodes.UserResetPasswordFailed);
            }

            return new ValidationResult();
        }

        public async Task<ValidationResult> DeleteUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new ValidationResult(
                    [
                        new ValidationFailure(nameof(userId), ErrorMessages.UserServiceUserNotFound, userId) { ErrorCode = ErrorCodes.NotFound},
                    ]);
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return result.ToValidationResult(ErrorCodes.UserDeletionFailed);
            }
            return new ValidationResult();
        }
    }
}
