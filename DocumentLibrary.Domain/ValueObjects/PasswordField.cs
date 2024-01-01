using DocumentLibrary.Domain.Common;
using DocumentLibrary.Domain.Common.Exceptions;
using System.Diagnostics;

namespace DocumentLibrary.Domain.ValueObjects
{
    public class PasswordField : ValueObject
    {
        public string Value { get; }

        [DebuggerStepThrough]
        public PasswordField(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidValueObjectStateException("Password can not be empty");
            }

            if (password.Length < 8)
            {
                throw new InvalidValueObjectStateException("Password lenght must be at least 8 characters");
            }

            Value = password;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}