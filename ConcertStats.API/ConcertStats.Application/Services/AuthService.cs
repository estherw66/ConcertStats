using ConcertStats.Application.Dtos.Mapper.Users;
using ConcertStats.Application.Dtos.Users;
using ConcertStats.Application.Interfaces.Repositories;
using ConcertStats.Application.Interfaces.Services;
using ConcertStats.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace ConcertStats.Application.Services;

public class AuthService(IUserRepository userRepository, IPasswordHasher<UserCredentials> hasher, IEncryptionService encryptionService) : IAuthService
{
    public async Task<UserDto> LoginAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Email and password cannot be empty");
        }
        // todo add jwt token
        
        var emailHash = await encryptionService.HashEmailAsync(email);
        
        var user = await FindUserByEmail(emailHash);
        
        if (!user.Credentials.IsActive)
        {
            throw new UnauthorizedAccessException("User account is not active");
        }
        
        if (await IsUserLockedOutAsync(emailHash))
        {
            throw new UnauthorizedAccessException("User account is locked out");
        }
        
        var passwordVerificationResult = hasher.VerifyHashedPassword(user.Credentials, user.Credentials.PasswordHash, password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            await IncrementFailedLoginAttemptsAsync(emailHash);
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var userDto = UserDtoMapper.ToDto(user);
        return userDto;
    }

    public Task LogoutAsync()
    {
        throw new NotImplementedException();
    }

    private async Task<User> FindUserByEmail(string email)
    {
        var user = await userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new InvalidOperationException($"User with email {email} not found");
        }
        return user;
    }
    
    private async Task<bool> IsUserLockedOutAsync(string email)
    {
        var user = await FindUserByEmail(email);

        if (!user.Credentials.IsLockedOut) return false;
        if (user.Credentials.LockoutEnd.HasValue && user.Credentials.LockoutEnd > DateTime.Now)
        {
            return true;
        }
            
        user.Credentials.IsLockedOut = false;
        user.Credentials.LockoutEnd = null;
        user.Credentials.Retry = 0;
            
        await userRepository.UpdateAsync(user);

        return false;
    }
    
    private async Task IncrementFailedLoginAttemptsAsync(string email)
    {
        // todo add thread safety
        var user = await FindUserByEmail(email);
        
        if (user.Credentials.LastFailedLogin != null && 
            user.Credentials.LastFailedLogin.Value.AddMinutes(15) < DateTime.UtcNow)
        {
            user.Credentials.Retry = 0;
        }
        
        user.Credentials.Retry++;
        user.Credentials.LastFailedLogin = DateTime.UtcNow;
        
        if (user.Credentials.Retry >= 5)
        {
            await LockoutUserAsync(email, TimeSpan.FromMinutes(15));
            throw new UnauthorizedAccessException("User account is locked out due to too many failed login attempts");
        }
        
        await userRepository.UpdateAsync(user);
    }

    private async Task LockoutUserAsync(string email, TimeSpan lockoutDuration)
    {
        var user = await FindUserByEmail(email);
        
        user.Credentials.IsLockedOut = true;
        user.Credentials.LockoutEnd = DateTime.UtcNow.Add(lockoutDuration);
        user.Credentials.Retry = 0;
        
        await userRepository.UpdateAsync(user);
    }
    
    // password reset and email confirmation

    public Task<string> GeneratePasswordResetTokenAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task ResetPasswordAsync(string email, string token, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task<string> GenerateEmailConfirmationTokenAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task ConfirmEmailAsync(string email, string token)
    {
        throw new NotImplementedException();
    }
}