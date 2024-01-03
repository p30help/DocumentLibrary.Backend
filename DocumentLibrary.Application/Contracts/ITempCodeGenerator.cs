namespace DocumentsLibrary.Application.Contracts
{
    public interface ITempCodeGenerator
    {
        string GenerateTempCode(Guid documentId, TimeSpan expirationTime);

        TempCode ValidateTempCode(string generatedLink);
    }

    public class TempCode
    {
        public bool IsValid { get; set; }

        public Guid? DocumentId { get; set; }
    }
}
