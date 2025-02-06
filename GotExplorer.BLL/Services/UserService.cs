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

        public async Task<ValidationWithEntityModel<UserDTO>> Login(LoginDTO loginDTO)
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

        public async Task<ValidationWithEntityModel<UserDTO>> Register(RegisterDTO registerDTO)
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
    }
}
