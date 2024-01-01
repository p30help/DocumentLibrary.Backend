using DocumentLibrary.Domain.Common.Exceptions;
using DocumentLibrary.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace DocumentLibrary.Domain.Users
{
    public class AppUser : IdentityUser<Guid>
    {
        private AppUser() { }

        public static AppUser Create(EmailField email, PasswordField password)
        {
            if (email == null)
            {
                throw new DomainStateException("Email must be defined");
            }

            if (password == null)
            {
                throw new DomainStateException("Password must be defined");
            }

            var user = new AppUser()
            {
                Email = email.Value,
                UserName = email.Value,
                PasswordHash = password.Value
            };

            return user;
        }
    }
}
