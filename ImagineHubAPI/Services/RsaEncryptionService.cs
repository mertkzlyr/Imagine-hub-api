using System.Security.Cryptography;
using System.Text;

namespace ImagineHubAPI.Services;

public class RsaEncryptionService
{
    private readonly RSA _privateRsa;
    private readonly RSA _publicRsa;

    public RsaEncryptionService(string privateKeyPath, string publicKeyPath)
    {
        _privateRsa = RSA.Create();
        _privateRsa.ImportFromPem(File.ReadAllText(privateKeyPath));

        _publicRsa = RSA.Create();
        _publicRsa.ImportFromPem(File.ReadAllText(publicKeyPath));
    }

    public string Encrypt(string plainText)
    {
        var bytesToEncrypt = Encoding.UTF8.GetBytes(plainText);
        var encryptedBytes = _publicRsa.Encrypt(bytesToEncrypt, RSAEncryptionPadding.OaepSHA256);
        return Convert.ToBase64String(encryptedBytes);
    }

    public string Decrypt(string encryptedBase64)
    {
        var encryptedBytes = Convert.FromBase64String(encryptedBase64);
        var decryptedBytes = _privateRsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}