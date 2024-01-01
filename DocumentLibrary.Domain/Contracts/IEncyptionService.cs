namespace DocumentLibrary.Domain.Contracts
{
    public interface IEncyptionService
    {
        string Encrypt(string key);

        string Decrypt(string encryptedTextAsBase64);
    }
}
