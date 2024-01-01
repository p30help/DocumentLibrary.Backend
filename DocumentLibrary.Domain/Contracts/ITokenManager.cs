using DocumentLibrary.Domain.Users;

namespace DocumentLibrary.Domain.Contracts
{
    public interface ITokenManager
    {
        string GenerateToken(AppUser user, UserRole[] roles);
    }
}
