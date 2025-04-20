using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace ImagineHubAPI.Helpers;

public static class RsaKeyUtils
{
    public static RsaSecurityKey GetPrivateKey(string path)
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(File.ReadAllText(path));
        return new RsaSecurityKey(rsa);
    }
    
    public static RsaSecurityKey GetPublicKey(string path)
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(File.ReadAllText(path));
        return new RsaSecurityKey(rsa);
    }
}