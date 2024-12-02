using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Extensions;
using GotExplorer.BLL.Services.Interfaces;
using GotExplorer.BLL.Services.Results;
using GotExplorer.BLL.Validators;
using GotExplorer.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GotExplorer.BLL.Services
{
    public class UserService : IUserService
    {
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

        public async Task<ServiceResult<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var serviceResult = new ServiceResult<UserDTO>();
            var validationResult = await _loginDtoValidator.ValidateAsync(loginDTO);

            if (!validationResult.IsValid)
            {
                serviceResult.Error = new Error(ErrorCodes.Invalid, validationResult);
                return serviceResult;
            }

            var user = await _userManager.FindByNameAsync(loginDTO.Username) ?? await _userManager.FindByEmailAsync(loginDTO.Username);

            if (user == null)
            {
                serviceResult.Error = new Error(ErrorCodes.Unauthorized, new ValidationResult() { 
                    Errors = new() { 
                        new ValidationFailure(nameof(loginDTO.Username), "Username not found and/or password incorrect"),
                        new ValidationFailure(nameof(loginDTO.Password), "Username not found and/or password incorrect"),
                    }
                });
                return serviceResult;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded)
            {
                serviceResult.Error = new Error(ErrorCodes.Unauthorized, new ValidationResult()
                {
                    Errors = new() {
                        new ValidationFailure(nameof(loginDTO.Username), "Username not found and/or password incorrect"),
                        new ValidationFailure(nameof(loginDTO.Password), "Username not found and/or password incorrect"),
                    }
                });
                return serviceResult;
            }

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Token = _jwtService.GenerateToken(user);
            serviceResult.ResultObject = userDto;
            return serviceResult;
        }

        public async Task<ServiceResult<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var serviceResult = new ServiceResult<UserDTO>();
            var validationResult = await _registerDtoValidator.ValidateAsync(registerDTO);

            if (!validationResult.IsValid)
            {
                serviceResult.Error = new Error(ErrorCodes.Invalid, validationResult); 
                return serviceResult;
            }

            var user = _mapper.Map<User>(registerDTO);

            var createdUser = await _userManager.CreateAsync(user, registerDTO.Password);
            
            if (!createdUser.Succeeded)
            {
                serviceResult.Error = new Error(ErrorCodes.UserCreationFailed, createdUser.ToValidationResult());
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                serviceResult.Error = new Error(ErrorCodes.RoleAssignmentFailed, roleResult.ToValidationResult());
            }

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Token = _jwtService.GenerateToken(user);
            serviceResult.ResultObject = userDto;   
            return serviceResult;
        }
             
        public async Task<ServiceResult> UpdateUserByIdAsync(UpdateUserDTO updateUserDTO)
        {
            var serviceResult = new ServiceResult();
            var validationResult = await _updateUserDtoValidator.ValidateAsync(updateUserDTO);
            if (!validationResult.IsValid)
            {
                serviceResult.Error = new Error(ErrorCodes.Invalid, validationResult);
                return serviceResult;
            }

            var user = await _userManager.FindByIdAsync(updateUserDTO.Id);

            if (user == null)
            {
                serviceResult.Error = new Error(ErrorCodes.NotFound, new ValidationResult()
                {
                    Errors = new() {
                        new ValidationFailure(nameof(updateUserDTO.Id), "User not found", updateUserDTO.Id),
                    }
                });
                return serviceResult;
            }
               
            _mapper.Map(updateUserDTO,user);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                serviceResult.Error = new Error(ErrorCodes.UserUpdateFailed, result.ToValidationResult());
                return serviceResult;
            }

            return serviceResult;
        }

        public async Task<ServiceResult> UpdatePasswordAsync(UpdateUserPasswordDTO updateUserPasswordDTO)
        {
            var serviceResult = new ServiceResult();
            var validationResult = await _updateUserPasswordDtoValidator.ValidateAsync(updateUserPasswordDTO);

            if (!validationResult.IsValid)
            {
                serviceResult.Error = new Error(ErrorCodes.Invalid, validationResult);
                return serviceResult;
            }

            var user = await _userManager.FindByIdAsync(updateUserPasswordDTO.Id);

            if (user == null)
            {
                serviceResult.Error = new Error(ErrorCodes.NotFound, new ValidationResult()
                {
                    Errors = new() {
                        new ValidationFailure(nameof(updateUserPasswordDTO.Id), "User not found", updateUserPasswordDTO.Id),
                    }
                });
                return serviceResult;;
            }
                
            var result = await _userManager.ChangePasswordAsync(user,updateUserPasswordDTO.CurrentPassword,updateUserPasswordDTO.NewPassword);

            if (!result.Succeeded)
            {
                serviceResult.Error = new Error(ErrorCodes.UserPasswordUpdateFailed, result.ToValidationResult());
            }
            return serviceResult;
        }

        public async Task<ServiceResult> GeneratePasswordResetLinkAsync(string email, string origin)
        {
            var serviceResult = new ServiceResult();
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                serviceResult.Error = new Error(ErrorCodes.NotFound, new ValidationResult()
                {
                    Errors = new() {
                        new ValidationFailure(nameof(email), "User not found",email),
                    }
                });
                return serviceResult;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = $"{origin}reset-password?id={user.Id}&token={token}";
            await _emailService.SendEmailAsync(email, "Your Password Reset Link", url);

            return serviceResult;
        }

        public async Task<ServiceResult> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var serviceResult = new ServiceResult();
            var validationResult = await _resetPasswordDtoValidator.ValidateAsync(resetPasswordDTO);
            if (!validationResult.IsValid)
            {
                serviceResult.Error = new Error(ErrorCodes.Invalid, validationResult);
                return serviceResult;
            }

            var user = await _userManager.FindByIdAsync(resetPasswordDTO.Id.ToString());
            if (user == null)
            {
                serviceResult.Error = new Error(ErrorCodes.NotFound, new ValidationResult()
                {
  
                    Errors = new() {
                        new ValidationFailure(nameof(resetPasswordDTO.Id), "User not found",resetPasswordDTO.Id),
                    }
                });
                return serviceResult;
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password);

            if (!result.Succeeded)
            {
                serviceResult.Error = new Error(ErrorCodes.UserResetPasswordFailed, result.ToValidationResult());
                return serviceResult;
            }

            return serviceResult;
        }

        public async Task<ServiceResult> DeleteUserByIdAsync(string userId)
        {
            var serviceResult = new ServiceResult();
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                serviceResult.Error = new Error(ErrorCodes.NotFound, new ValidationResult()
                {
                    Errors = new() {
                        new ValidationFailure(nameof(userId), "User not found", userId),
                    }
                });
                return serviceResult;
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                serviceResult.Error = new Error(ErrorCodes.UserDeletionFailed, result.ToValidationResult());
                return serviceResult;
            }
            return serviceResult;
        }
    }
}
