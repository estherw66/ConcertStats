using ConcertStats.Application.Dtos.Users;
using ConcertStats.Core.Entities;

namespace ConcertStats.Application.Interfaces.Services;

public interface IAuthService
{
     Task<UserDto> LoginAsync(string email, string password);
     Task LogoutAsync();
     Task<string> GeneratePasswordResetTokenAsync(string email);
     Task ResetPasswordAsync(string email, string token, string newPassword);
     Task<string> GenerateEmailConfirmationTokenAsync(string email);
     Task ConfirmEmailAsync(string email, string token);
}