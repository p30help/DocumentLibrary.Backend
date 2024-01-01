using Microsoft.AspNetCore.Identity;

namespace DocumentLibrary.Domain.Users
{
    public class AppRole : IdentityRole<Guid>
    {
        private AppRole() { }

        public static AppRole Create(UserRole userRole)
        {
            var user = new AppRole()
            {
                Id = Guid.NewGuid(),
                Name = userRole.ToString()
            };

            return user;
        }
    }
}
