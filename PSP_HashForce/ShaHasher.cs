namespace PSP_HashForce;

using System;
using System.Security.Cryptography;
using System.Text;

public class ShaHasher
{
    private readonly SHA256 _sha256 = SHA256.Create();

    public string GetStringSha256Hash(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        byte[] textData = Encoding.UTF8.GetBytes(text);
        byte[] hash = _sha256.ComputeHash(textData);
        return BitConverter.ToString(hash).Replace("-", string.Empty);
    }
}