using GotExplorer.API.Extensions;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services;
using GotExplorer.BLL.Services.Interfaces;
using GotExplorer.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var result = await _userService.Register(registerDTO);
            return result.ToActionResult<UserDTO>();
        }

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="loginDTO">Login credentials</param>
        /// <response code="200">Authentication succeeded; returns a JWT token for the authenticated user.</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="401">Authentication failed due to invalid username (or email) and/or password</response>
        /// <response code="500">An unexpected error occurred on the server</response>        
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _userService.Login(loginDTO);
           
            return result.ToActionResult<UserDTO>();
        }

        // TODO: Delete endpoint.
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("JwtTest")]
        public IActionResult TestJwtAuth()
        {
            return Ok("Authentication successful");
        }
    }
}
