using FluentValidation.Results;
using GotExplorer.API.Extensions;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services;
using GotExplorer.BLL.Services.Interfaces;
using GotExplorer.BLL.Services.Results;
using GotExplorer.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace GotExplorer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="registerDTO">User data for registration.</param>
        /// <response code="200">Registration succeeded, and a JWT token is returned for the new user.</response>
        /// <response code="400">Registration failed due to invalid input.</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var result = await _userService.RegisterAsync(registerDTO);
            return result.ToActionResult<UserDTO>();
        }

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="loginDTO">Login credentials</param>
        /// <response code="200">Authentication succeeded; returns a JWT token for the authenticated user.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">Authentication failed due to invalid username (or email) and/or password.</response>
        /// <response code="500">An unexpected error occurred on the server.</response>        
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _userService.LoginAsync(loginDTO);
           
            return result.ToActionResult<UserDTO>();
        }

        /// <summary>
        /// Update user data by id stored in jwt claims. Require Authorization.
        /// </summary>
        /// <param name="updateUserDTO">The data to update the user's profile</param>
        /// <response code="200">Successfully updated the user data.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="401">Authentication failed due to invalid JWT.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">An unexpected error occurred on the server.</response>        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> UpdateUserById([FromBody] UpdateUserDTO updateUserDTO)
        {
            updateUserDTO.Id = User.GetClaimValue("Id");
            var result = await _userService.UpdateUserByIdAsync(updateUserDTO);
            return result.ToActionResult();
        }

        /// <summary>
        /// Update user password by id stored in jwt claims. Require Authorization.
        /// </summary>
        /// <param name="updateUserPasswordDTO">The data to update the user's password</param>
        /// <response code="200">Successfully updated the user password.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="401">Authentication failed due to invalid JWT.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">An unexpected error occurred on the server.</response>        
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("update-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordDTO updateUserPasswordDTO)
        {
            updateUserPasswordDTO.Id = User.GetClaimValue("Id");
            var result = await _userService.UpdatePasswordAsync( updateUserPasswordDTO);
            return result.ToActionResult();
        }

        /// <summary>
        /// Generates user password reset link and send it to user's email.
        /// </summary>
        /// <param name="generatePasswordResetLinkDTO">The data to generate the user's password reset link</param>
        /// <response code="204">Successfully generated user password reset link.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">An unexpected error occurred on the server.</response>        
        [HttpPut("password-reset-link")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> GeneratePasswordResetLink([FromBody] GeneratePasswordResetLinkDTO generatePasswordResetLinkDTO)
        {
            var result = await _userService.GeneratePasswordResetLinkAsync(generatePasswordResetLinkDTO.Email, $"{Request.Scheme}://{Request.Host}{Url.Content("~/")}");
            if (result.IsValid)
            {
                return NoContent();
            }
            return result.ToActionResult();
        }

        /// <summary>
        /// Reset user password by generated reset password token
        /// </summary>
        /// <param name="resetPasswordDTO">The data to reset the user's password</param>
        /// <response code="200">Successfully reseted the user's password.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">An unexpected error occurred on the server.</response>        
        [HttpPut("password-reset")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var result = await _userService.ResetPasswordAsync(resetPasswordDTO);
            return result.ToActionResult();
        }

        /// <summary>
        /// Delete user by id stored in jwt claims. Require Authorization
        /// </summary>
        /// <response code="200">Successfully deleted user</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="401">Authentication failed due to invalid JWT.</response>
        /// <response code="404">User not found</response>
        /// <response code="500">An unexpected error occurred on the server.</response>   
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = User.GetClaimValue("Id");
            var result = await _userService.DeleteUserByIdAsync(userId);
            return result.ToActionResult();
        }
    }
}
