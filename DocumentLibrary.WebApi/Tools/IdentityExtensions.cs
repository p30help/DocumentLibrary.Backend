using System.Security.Claims;

namespace DocumentLibrary.WebApi.Tools
{
    public static class IdentityExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new Exception("User is not valid");
            return Guid.Parse(claim!.Value);
        }

        public static string? GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
