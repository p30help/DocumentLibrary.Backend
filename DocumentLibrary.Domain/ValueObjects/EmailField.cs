using DocumentLibrary.Domain.Common;
using DocumentLibrary.Domain.Common.Exceptions;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DocumentLibrary.Domain.ValueObjects
{
    public class EmailField : ValueObject
    {
        public string Value { get; }

        [DebuggerStepThrough]
        public EmailField(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidValueObjectStateException("Email can not be empty");
            }

            var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

            var regex = new Regex(pattern);

            if (!regex.IsMatch(email))
            {
                throw new InvalidValueObjectStateException("Email is not correct");
            }

            Value = email.ToLower();
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