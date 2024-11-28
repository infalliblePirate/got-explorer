using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Extensions;
using GotExplorer.BLL.Services.Interfaces;
using GotExplorer.BLL.Services.Results;
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
        public UserService(
            IJwtService jwtService, 
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IMapper mapper, 
            IValidator<LoginDTO> loginDtoValidator, 
            IValidator<RegisterDTO> registerDtoValidator,
            IValidator<UpdateUserDTO> updateUserDtoValidator,
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
        }

        public async Task<ServiceResult<UserDTO>> Login(LoginDTO loginDTO)
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

        public async Task<ServiceResult<UserDTO>> Register(RegisterDTO registerDTO)
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
             
        public async Task UpdateUserByIdAsync(UpdateUserDTO updateUserDTO)
        {
            var validationResult = await _updateUserDtoValidator.ValidateAsync(updateUserDTO);
            if (!validationResult.IsValid)
            {
                var ex = new BadRequestException("Validation failed");
                ex.Data["errors"] = validationResult.Errors;
                throw ex;
            }

            var user = await _userManager.FindByIdAsync(updateUserDTO.Id);

            if (user == null)
                throw new NotFoundException("User not found");

            _mapper.Map(updateUserDTO,user);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var ex = new BadRequestException("Updating user failed");
                ex.Data["errors"] = result.Errors;
                throw ex;
            }
        }

        public async Task UpdatePasswordAsync(UpdateUserPasswordDTO updateUserPasswordDTO)
        {
            var user = await _userManager.FindByIdAsync(updateUserPasswordDTO.Id);

            if (user == null)
                throw new NotFoundException("User not found");

            var result = await _userManager.ChangePasswordAsync(user,updateUserPasswordDTO.CurrentPassword,updateUserPasswordDTO.NewPassword);

            if (!result.Succeeded)
            {
                var ex = new BadRequestException("Updating password failed");
                ex.Data["errors"] = result.Errors;
                throw ex;
            }
        }

        public async Task GeneratePasswordResetLinkAsync(string email, string origin)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                throw new NotFoundException("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = $"{origin}reset-password?id={user.Id}&token={token}";
            await _emailService.SendEmailAsync(email, "Your Password Reset Link", url);
        }

        public async Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await _userManager.FindByIdAsync(resetPasswordDTO.Id.ToString());
            if (user == null)
                throw new NotFoundException("User not found");

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Token);

            if (!result.Succeeded)
            {
                var ex = new BadRequestException("Resetting password failed");
                ex.Data["errors"] = result.Errors;
                throw ex;
            }
        }

        public async Task DeleteUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found");

            var result = await _userManager.DeleteAsync(user);

            var rolesForUser = await _userManager.GetRolesAsync(user);


            if (rolesForUser.Count() > 0)
            {
                foreach (var item in rolesForUser.ToList())
                {
                    await _userManager.RemoveFromRoleAsync(user, item);
                }
            }

            await _userManager.DeleteAsync(user);
  
            if (!result.Succeeded)
            {
                var ex = new BadRequestException("Deleting user failed");
                ex.Data["errors"] = result.Errors;
                throw ex;
            }
        }
    }
}
