using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services;
using GotExplorer.BLL.Services.Interfaces;
using GotExplorer.DAL.Models;
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
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager,IJwtService jwtService, SignInManager<User> signInManager)
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="registerDTO">User data for registration</param>
        /// <response code="200">Returns the JWT token</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            // TODO: add user registration and validation.
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = new User
                {
                    UserName = registerDTO.Username,
                    Email = registerDTO.Email,
                };


                return Ok(_jwtService.GenerateToken(user));

            }
            catch (Exception e)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="loginDTO">Login credentials</param>
        /// <response code="200">Returns the JWT token</response>
        /// <response code="401">Returns if invalid username and/or password</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 401)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            // TODO: add user login validation.
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // TODO: Change to actual user, after adding database.
                return Ok( _jwtService.GenerateToken(new User { Email = loginDTO.Username, UserName = loginDTO.Username}));
            }
            catch 
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
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
