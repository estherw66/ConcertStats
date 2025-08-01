using System.Text.RegularExpressions;
using ConcertStats.Application.Dtos.Mapper.Users;
using ConcertStats.Application.Dtos.Request.Users;
using ConcertStats.Application.Dtos.Users;
using ConcertStats.Application.Interfaces.Repositories;
using ConcertStats.Application.Interfaces.Services;
using ConcertStats.Core.Entities;
using ConcertStats.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ConcertStats.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IEncryptionService encryptionService,
    ILogger<UserService> logger,
    IPasswordHasher<UserCredentials> hasher)
    : IUserService
{
    public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.FullName))
        {
            throw new ArgumentException("Please fill in all required fields.");
        }
        
        ValidatePasswordStrength(request.Password);
        ValidateEmail(request.Email);
        await VerifyUniqueEmail(request.Email);
        ValidateUsername(request.Username);
        
        var userByUsername = await userRepository.GetByUsernameAsync(request.Username);
        if (userByUsername != null)
        {
            throw new InvalidOperationException("Username already exists.");
        }
            
        var user = CreateUserRequestDtoMapper.ToEntity(request);
            
        var hashedPassword = hasher.HashPassword(user.Credentials, request.Password);
        user.Credentials.PasswordHash = hashedPassword;
            
        var encryptedEmail = await encryptionService.EncryptAsync(request.Email);
        user.Credentials.Email = encryptedEmail;
        
        var emailHash = await encryptionService.HashEmailAsync(request.Email);
        user.Credentials.EmailHash = emailHash;
        
        user.Roles.Add(new UserRoleJoin {UserRole = UserRole.User});    
        
        await userRepository.CreateAsync(user);

        var userDto = UserDtoMapper.ToDto(user);

        return userDto;
    }

    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }
        
        var decryptedEmail = await encryptionService.DecryptAsync(user.Credentials.Email);
        var userDto = UserDtoMapper.ToDto(user);
        userDto.Email = decryptedEmail;
        
        return userDto;
    }

    public async Task<UserProfileDto> GetUserProfileByUsernameAsync(string username)
    {
        ValidateUsername(username);
        
        var user = await userRepository.GetByUsernameAsync(username);
        if (user == null || !user.Credentials.IsActive)
        {
            throw new InvalidOperationException("User not found.");
        }

        // todo: check privacy settings
        
        var userDto = UserProfileDtoMapper.ToDto(user);
        return userDto;
    }

    public async Task<IEnumerable<UserResultDto>> GetUsersAsync(int pageNumber, int pageSize, string searchQuery)
    {
        var skip = (pageNumber - 1) * pageSize;
        
        var users = await userRepository.GetAllAsync(skip, pageSize, searchQuery);
        var userDtos = users.Select(UserResultMapper.ToDto).ToList();
        
        return userDtos;
    }

    public Task<UserDto> UpdateUserAsync(int userId, UpdateUserRequest request)
    {
        
        throw new NotImplementedException();
    }

    public async Task<UserDto> UpdateUserProfileAsync(int userId, UpdateUserProfileRequest request)
    {
        if (string.IsNullOrEmpty(request.FullName))
        {
            throw new ArgumentException("Please fill in all required fields.");
        }

        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }
        
        user.Profile.FullName = request.FullName;
        if (!string.IsNullOrEmpty(request.Bio))
        {
            user.Profile.Bio = request.Bio;
        }
        if (!string.IsNullOrEmpty(request.ProfilePictureUrl))
        {
            user.Profile.ProfilePictureUrl = request.ProfilePictureUrl;
        }
        if (!string.IsNullOrEmpty(request.Location))
        {
            user.Profile.Location = request.Location;
        }
        
        await userRepository.UpdateAsync(user);
        var userDto = UserDtoMapper.ToDto(user);
        return userDto;
    }

    public async Task<UserDto> UpdateUserCredentialsAsync(int userId, UpdateUserCredentialsRequest request)
    {
        ValidateEmail(request.Email);
        
        await VerifyUniqueEmail(request.Email);
        
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NullReferenceException("User not found."); // todo: use custom exception
        }
        
        var encryptedEmail = await encryptionService.EncryptAsync(request.Email);
        
        var hashedEmail = await encryptionService.HashEmailAsync(request.Email);
        
        user.Credentials.Email = encryptedEmail;
        user.Credentials.EmailHash = hashedEmail;
        
        user.Credentials.EmailConfirmed = false;
        
        await userRepository.UpdateAsync(user);
        
        var userDto = UserDtoMapper.ToDto(user);
        
        return userDto;
    }

    public async Task<UserDto> UpdateUserSettingsAsync(int userId, UpdateUserSettingsRequest request)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NullReferenceException("User not found."); // todo: use custom exception
        }
        
        user.Settings.Language = ParseEnum<Language>(request.Language, "Language");
        user.Settings.Theme = ParseEnum<Theme>(request.Theme, "Theme");
        user.Settings.PrivacySettings = ParseEnum<PrivacySettings>(request.PrivacySettings, "Privacy Settings");
        
        await userRepository.UpdateAsync(user);
        
        var userDto = UserDtoMapper.ToDto(user);
        return userDto;
    }

    public async Task UpdateUserPasswordAsync(int userId, UpdateUserPasswordRequest request)
    {
        if (string.IsNullOrEmpty(request.OldPassword) || string.IsNullOrEmpty(request.NewPassword))
        {
            throw new ArgumentException("Please fill in all required fields.");
        }
        
        ValidatePasswordStrength(request.NewPassword);
        
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NullReferenceException("User not found."); // todo: use custom exception
        }

        var passwordVerificationResult = hasher.VerifyHashedPassword(user.Credentials, user.Credentials.PasswordHash, request.OldPassword);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new InvalidOperationException("Incorrect credentials.");
        }
        
        if (request.NewPassword == request.OldPassword)
        {
            throw new ArgumentException("New password cannot be the same as the old password.");
        }
        
        var hashedPassword = hasher.HashPassword(user.Credentials, request.NewPassword);
        user.Credentials.PasswordHash = hashedPassword;
        
        await userRepository.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NullReferenceException("User not found."); // todo: use custom exception
        }
        
        await userRepository.DeleteAsync(userId);
    }

    public async Task DeactivateUserAsync(int userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NullReferenceException("User not found."); // todo: use custom exception
        }
        
        user.Credentials.IsActive = false;
        
        await userRepository.UpdateAsync(user);
    }
    
    private void ValidatePasswordStrength(string password)
    {
        if (password.Length < 8)
        {
            throw new ArgumentException("Password must be at least 8 characters long.");
        }
        if (!password.Any(char.IsUpper))
        {
            throw new ArgumentException("Password must contain at least one uppercase letter.");
        }
        if (!password.Any(char.IsLower))
        {
            throw new ArgumentException("Password must contain at least one lowercase letter.");
        }
        if (!password.Any(char.IsDigit))
        {
            throw new ArgumentException("Password must contain at least one digit.");
        }
        if (!password.Any(ch => "!@#$%^&*()_+[]{}|;':\",.<>?`~".Contains(ch)))
        {
            throw new ArgumentException("Password must contain at least one special character.");
        }
        if (password.Any(char.IsWhiteSpace))
        {
            throw new ArgumentException("Password must not contain any spaces.");
        }
    }

    private void ValidateEmail(string email)
    {
        var emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        if (!Regex.IsMatch(email, emailPattern))
        {
            throw new ArgumentException("Invalid email format.");
        }
    }
    
    private async Task VerifyUniqueEmail(string email)
    {
        var inputHash = await encryptionService.HashEmailAsync(email);
        var user = await userRepository.GetByEmailAsync(inputHash);
        if (user != null)
        {
            throw new InvalidOperationException("Email already exists.");
        }
    }
    
    private void ValidateUsername(string username)
    {
        var usernamePattern = @"^[a-zA-Z0-9_]{3,20}$"; 
        if (!Regex.IsMatch(username, usernamePattern))
        {
            throw new ArgumentException("Username invalid");
        }
    }
    
    private TEnum ParseEnum<TEnum>(string value, string fieldName) where TEnum : struct
    {
        if (Enum.TryParse<TEnum>(value, true, out var result))
        {
            return result;
        }
        throw new ArgumentException($"Invalid {fieldName} specified.");
    }
}