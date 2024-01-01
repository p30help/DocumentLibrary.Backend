using DocumentLibrary.Domain.Contracts;

namespace DocumentLibrary.Infrastructure
{
    public class DateProvider : IDateProvider
    {
        public DateTimeOffset Now => DateTimeOffset.Now;

        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
