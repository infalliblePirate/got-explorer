using System.Security.Claims;

namespace GotExplorer.API.Extensions
{
    public static class ClaimsExtensions
    {
        public static string? GetClaimValue(this ClaimsPrincipal user, string claimType)
        {
            return user?.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;
        }
    }
}
