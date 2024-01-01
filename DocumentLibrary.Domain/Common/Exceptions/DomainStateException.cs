namespace DocumentLibrary.Domain.Common.Exceptions
{
    public class DomainStateException : Exception
    {
        public string[] Parameters { get; set; }

        public DomainStateException(string message, params string[] parameters) : base(message)
        {
            Parameters = parameters;
        }
    }
}