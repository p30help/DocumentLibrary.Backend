using DocumentLibrary.Domain.Users;

namespace DocumentLibrary.Domain.Contracts
{
    public interface IUserRepository
    {
        public Task AddUser(AppUser user);

        public Task<bool> ExistUser(string email);

        Task<AppUser?> GetUser(Guid userId);

        Task<AppUser?> GetUserWithPassword(string email, string password);

        Task<UserRole[]> GetUserRoles(string email);
    }
}
