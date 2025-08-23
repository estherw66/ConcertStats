using ConcertStats.Application.Dtos.Users;
using ConcertStats.Core.Entities;
using ConcertStats.Core.Entities.User;

namespace ConcertStats.Application.Dtos.Mapper.Users;

public static class UserProfileDtoMapper
{
    public static UserProfileDto ToDto(User user)
    {
        return new UserProfileDto
        {
            Username = user.Username,
            FullName = user.Profile.FullName,
            ProfilePictureUrl = user.Profile.ProfilePictureUrl ?? string.Empty,
            Bio = user.Profile.Bio ?? string.Empty,
            Location = user.Profile.Location ?? string.Empty,
            PrivacySettings = user.Settings.PrivacySettings.ToString()
        };
    }
}