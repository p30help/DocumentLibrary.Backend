using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Users;
using DocumentLibrary.Infrastructure.Minio;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocumentLibrary.Infrastructure.Token;

public class TokenManager : ITokenManager
{
    private readonly JwtConfiguration jwtConfiguration;
    private readonly IDateProvider dateProvider;

    public TokenManager(JwtConfiguration jwtConfiguration, IDateProvider dateProvider)
    {
        this.jwtConfiguration = jwtConfiguration;
        this.dateProvider = dateProvider;
    }

    public string GenerateToken(AppUser user, UserRole[] roles)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (roles == null)
            throw new ArgumentNullException(nameof(roles));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key!));
        var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);        

        var token = new JwtSecurityToken(
            issuer: jwtConfiguration.Issuer,
            audience: jwtConfiguration.Audience,
            claims: GenerateClaims(user, roles),
            expires: dateProvider.Now.AddSeconds(jwtConfiguration.ExpireIn).LocalDateTime,
            signingCredentials: credential);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private List<Claim> GenerateClaims(AppUser user, UserRole[] roles)
    {
        var userClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        foreach (var role in roles)
        {
            userClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        }

        return userClaims;
    }
}
