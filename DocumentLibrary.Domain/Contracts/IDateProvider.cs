namespace DocumentLibrary.Domain.Contracts
{
    public interface IDateProvider
    {
        public DateTimeOffset Now { get; }

        public DateTimeOffset UtcNow { get; }
    }
}
