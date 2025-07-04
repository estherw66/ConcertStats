namespace ConcertStats.Application.Interfaces.Services;

public interface IEncryptionService
{
    Task<string> EncryptAsync(string data);
    Task<string> DecryptAsync(string encryptedData);
}