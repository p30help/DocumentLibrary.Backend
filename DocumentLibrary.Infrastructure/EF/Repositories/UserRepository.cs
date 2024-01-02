using DocumentLibrary.Domain.Contracts;
using DocumentLibrary.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DocumentLibrary.Infrastructure.EF.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<AppUser> userManager;
    private readonly RoleManager<AppRole> roleManager;
    private readonly AppDbContext dbContext;

    public UserRepository(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, AppDbContext dbContext)
    {
        this.userManager = userManager;
        this.dbContext = dbContext;
        this.roleManager = roleManager;
    }

    public async Task AddUser(AppUser user)
    {
        var result = await userManager.CreateAsync(user, user.PasswordHash!);

        if (!result.Succeeded)
            throw new Exception("Error on creating user, Errors:" + string.Join(", ", result.Errors.Select(x => x.Description).ToArray()));

        var role = await TryToCreateUserRole(UserRole.User);

        await userManager.AddToRoleAsync(user, role.Name!);
    }

    private async Task<AppRole> TryToCreateUserRole(UserRole userRole)
    {
        var role = await roleManager.FindByNameAsync(userRole.ToString());
        if (role == null)
        {
            role = AppRole.Create(UserRole.User);

            await roleManager.CreateAsync(role);
        }

        return role;
    }

    public async Task<bool> ExistUser(string email)
    {
        return await dbContext.Users.AnyAsync(x => x.Email == email.ToLower());
    }

    public async Task<AppUser?> GetUserWithPassword(string email, string password)
    {
        if (email == null || password == null)
            return null;

        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
            return null;

        var passwordRes = await userManager.CheckPasswordAsync(user, password);
        if (!passwordRes)
            return null;

        return user;
    }

    public async Task<UserRole[]> GetUserRoles(string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
            throw new Exception("User not found");

        var roles = await userManager.GetRolesAsync(user);
        return roles.Select(x => Enum.Parse<UserRole>(x)).ToArray();
    }

    public async Task<AppUser?> GetUser(Guid userId)
    {
        return await userManager.FindByIdAsync(userId.ToString());
    }
}
