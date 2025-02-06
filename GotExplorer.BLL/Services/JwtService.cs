using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GotExplorer.DAL.Entities;
using GotExplorer.BLL.Services.Interfaces;
using GotExplorer.BLL.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
namespace GotExplorer.BLL.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<User> _userManager;
        public JwtService(IOptions<JwtOptions> jwtOptions, UserManager<User> userManager)
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
        }

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                Expires = DateTime.Now.AddDays(_jwtOptions.Expires),
                SigningCredentials = credentials,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private ClaimsIdentity GenerateClaims(User user)
        {
            var ci = new ClaimsIdentity();
            ci.AddClaim(new Claim("Id", user.Id.ToString()));
            ci.AddClaim(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            ci.AddClaim(new Claim(JwtRegisteredClaimNames.Name, user.UserName));
            var roles = _userManager.GetRolesAsync(user).Result; 
            foreach (var role in roles)
            {
                ci.AddClaim(new Claim(ClaimTypes.Role, role));
            }
           
            return ci;
        }
    }
}
