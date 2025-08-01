using ConcertStats.Core.Entities;

namespace ConcertStats.Application.Interfaces.Services;

public interface IEncryptionService
{
    Task<string> EncryptAsync(string data);
    Task<string> DecryptAsync(string encryptedData);
    Task<string> HashEmailAsync(string email);
}