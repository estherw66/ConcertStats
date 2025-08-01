using ConcertStats.Application.Dtos.Request.Users;
using ConcertStats.Application.Dtos.Users;

namespace ConcertStats.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(CreateUserRequest request);
    Task<UserDto> GetUserByIdAsync(int userId);
    Task<UserProfileDto> GetUserProfileByUsernameAsync(string username);
    Task<IEnumerable<UserResultDto>> GetUsersAsync(int pageNumber, int pageSize, string searchQuery);
    Task<UserDto> UpdateUserAsync(int userId, UpdateUserRequest request);
    Task<UserDto> UpdateUserProfileAsync(int userId, UpdateUserProfileRequest request);
    Task<UserDto> UpdateUserCredentialsAsync(int userId, UpdateUserCredentialsRequest request);
    Task<UserDto> UpdateUserSettingsAsync(int userId, UpdateUserSettingsRequest request);
    Task UpdateUserPasswordAsync(int userId, UpdateUserPasswordRequest request);
    Task DeleteUserAsync(int userId);
    Task DeactivateUserAsync(int userId);
}