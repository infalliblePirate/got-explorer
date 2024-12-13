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
        private readonly IValidator<RegisterDTO> _registerDtoValidator;
        private readonly IValidator<LoginDTO> _loginDtoValidator;
        public UserService(
            IJwtService jwtService, 
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IMapper mapper, 
            IValidator<LoginDTO> loginDtoValidator, 
            IValidator<RegisterDTO> registerDtoValidator)
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _loginDtoValidator = loginDtoValidator;
            _registerDtoValidator = registerDtoValidator;
        }

        public async Task<ServiceResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var validationResult = await _loginDtoValidator.ValidateAsync(loginDTO);

            if (!validationResult.IsValid)
            {
                return ServiceResult<UserDTO>.Failure(validationResult);
            }

            var user = await _userManager.FindByNameAsync(loginDTO.Username) ?? await _userManager.FindByEmailAsync(loginDTO.Username);
            
            if (user == null)
            {
                return ServiceResult<UserDTO>.Failure(new ValidationResult()
                {
                    Errors = new() {
                        new ValidationFailure(nameof(loginDTO.Username), "Username is incorrect",loginDTO.Username) { ErrorCode = ErrorCodes.Unauthorized },
                    }
                });
            }
           
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded)
            {
                return ServiceResult<UserDTO>.Failure(new ValidationResult()
                {
                    Errors = new() {
                        new ValidationFailure(nameof(loginDTO.Password), "Password is incorrect",loginDTO.Password) { ErrorCode = ErrorCodes.Unauthorized },
                    }
                });
            }

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Token = _jwtService.GenerateToken(user);
            return ServiceResult<UserDTO>.Success(userDto);
        }

        public async Task<ServiceResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var serviceResult = new ServiceResult<UserDTO>();
            var validationResult = await _registerDtoValidator.ValidateAsync(registerDTO);

            if (!validationResult.IsValid)
            {
                return ServiceResult<UserDTO>.Failure(validationResult);
            }

            var user = _mapper.Map<User>(registerDTO);

            var createdUser = await _userManager.CreateAsync(user, registerDTO.Password);
            
            if (!createdUser.Succeeded)
            {
                return ServiceResult<UserDTO>.Failure(createdUser.ToValidationResult(ErrorCodes.UserCreationFailed));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                return ServiceResult<UserDTO>.Failure(roleResult.ToValidationResult(ErrorCodes.RoleAssignmentFailed));
            }

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Token = _jwtService.GenerateToken(user);
            return ServiceResult<UserDTO>.Success(userDto);
        }
    }
}
