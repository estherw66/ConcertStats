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
        
        // validate password strength
        ValidatePasswordStrength(request.Password);
        
        // validate email format
        ValidateEmail(request.Email);
        
        // check if email already exists
        await ValidateEmailHash(request.Email);
        
        // validate username format
        ValidateUsername(request.Username);
        
        // check if username already exists
        var userByUsername = await userRepository.GetByUsernameAsync(request.Username);
        if (userByUsername != null)
        {
            throw new InvalidOperationException("Username already exists.");
        }
            
        // map request to entity
        var user = CreateUserRequestDtoMapper.ToEntity(request);
            
        // hash password
        var hashedPassword = hasher.HashPassword(user.Credentials, request.Password);
        user.Credentials.PasswordHash = hashedPassword;
            
        // encrypt sensitive data
        var encryptedEmail = await encryptionService.EncryptAsync(request.Email);
        user.Credentials.Email = encryptedEmail;
        
        // store hash for email verification
        var emailHash = await encryptionService.HashEmailAsync(request.Email);
        user.Credentials.EmailHash = emailHash;
        
        // set user role
        user.Roles.Add(new UserRoleJoin {UserRole = UserRole.User});    
        
        // save entity
        await userRepository.CreateAsync(user);

        // map entity to DTO
        var userDto = UserDtoMapper.ToDto(user);

        // return DTO
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

    public Task<UserDto> UpdateUserProfileAsync(int userId, UpdateUserProfileRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> UpdateUserCredentialsAsync(int userId, UpdateUserCredentialsRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> UpdateUserSettingsAsync(int userId, UpdateUserSettingsRequest request)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserPasswordAsync(int userId, UpdateUserPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task DeactivateUserAsync(int userId)
    {
        throw new NotImplementedException();
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
    
    private async Task ValidateEmailHash(string email)
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
}