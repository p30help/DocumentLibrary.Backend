using DocumentLibrary.Domain.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace DocumentLibrary.Infrastructure.TempLink;

public class EncyptionService : IEncyptionService
{
    private readonly string key;
    private readonly string keyAsBase64;
    private readonly string ivAsBase64;

    public EncyptionService(string encryptionKey, string ivBased64)
    {
        ivAsBase64 = ivBased64;
        key = encryptionKey;
        byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
        keyAsBase64 = Convert.ToBase64String(keyBytes);
    }

    public string Decrypt(string encryptedTextAsBase64)
    {
        using var aes = Aes.Create();
        
        byte[] iv = Convert.FromBase64String(ivAsBase64);
        byte[] keyBytes = Convert.FromBase64String(keyAsBase64);
        byte[] fromBase64ToBytes = Convert.FromBase64String(encryptedTextAsBase64);

        // Decrypt the text
        var decryptor = aes.CreateDecryptor(keyBytes, iv);
        byte[] decryptedBytes = decryptor.TransformFinalBlock(fromBase64ToBytes, 0, fromBase64ToBytes.Length);

        return Encoding.UTF8.GetString(decryptedBytes);
    }

    public string Encrypt(string text)
    {            
        using var aes = Aes.Create();

        aes.IV = Convert.FromBase64String(ivAsBase64);
        aes.GenerateKey();            
        byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, aes.Key.Length));
        aes.Key = keyBytes;            

        // Encrypt the text
        byte[] textBytes = Encoding.UTF8.GetBytes(text);
        var cryptor = aes.CreateEncryptor();
        byte[] encryptedBytes = cryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
        var encryptedTextAsBase64 = Convert.ToBase64String(encryptedBytes);

        return encryptedTextAsBase64;
    }
}
