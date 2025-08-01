using System.Security.Cryptography;
using System.Text;
using ConcertStats.Application.Configuration;
using ConcertStats.Application.Interfaces.Services;
using ConcertStats.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ConcertStats.Application.Services;

public class EncryptionService(IOptions<EncryptionConfig> options) : IEncryptionService
{
    private readonly string _password = options.Value.Password;
    private const int KeySize = 32;
    private const int SaltSize = 16;
    private const int IvSize = 16;
    private const int HashSize = 32;
    
    private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;
    
    private static byte[] DeriveKeyFromPassword(string password, byte[] salt)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password), 
            salt, 
            10000, 
            HashAlgorithmName.SHA384, 
            KeySize);
    }
    
    public async Task<string> EncryptAsync(string data)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] key = DeriveKeyFromPassword(_password, salt);
        
        using var aes = Aes.Create();
        aes.Key = key;
        
        aes.GenerateIV();
        var iv = aes.IV;

        using var memoryStream = new MemoryStream();
        memoryStream.Write(salt, 0, salt.Length); // add salt to encrypted data
        memoryStream.Write(iv, 0, iv.Length); // add iv to encrypted data
        await using var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(aes.Key, iv), CryptoStreamMode.Write);
        await using var writer = new StreamWriter(cryptoStream);
        await writer.WriteAsync(data);
        await writer.FlushAsync();
        await cryptoStream.FlushFinalBlockAsync();
            
        var buf = memoryStream.ToArray();
        return Convert.ToBase64String(buf);
    }

    public async Task<string> DecryptAsync(string encryptedData)
    {
        var bytes = Convert.FromBase64String(encryptedData);
        
        using var aes = Aes.Create();
        using var memoryStream = new MemoryStream(bytes);
        var salt = new byte[SaltSize];
        var saltRead = 0;
        while (saltRead < SaltSize)
        {
            var read = await memoryStream.ReadAsync(salt.AsMemory(saltRead, SaltSize - saltRead), CancellationToken.None);
            if (read == 0)
                throw new InvalidOperationException("Unexpected end of stream while reading salt.");
            saltRead += read;
        }
        
        var iv = new byte[IvSize];
        var ivRead = 0;
        while (ivRead < IvSize)
        {
            var read = await memoryStream.ReadAsync(iv.AsMemory(ivRead, IvSize - ivRead), CancellationToken.None);
            if (read == 0)
                throw new InvalidOperationException("Unexpected end of stream while reading IV.");
            ivRead += read;
        }

        var key = DeriveKeyFromPassword(_password, salt);

        await using var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read);
        using var reader = new StreamReader(cryptoStream);
        var decryptedData = await reader.ReadToEndAsync();
        
        return decryptedData;
    }

    public Task<string> HashEmailAsync(string email)
    {
        using var sha512 = SHA512.Create();
        var bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(email.Trim().ToLowerInvariant()));
        var hash = Convert.ToHexString(bytes);
        return Task.FromResult(hash);
    }
}