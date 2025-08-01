using ConcertStats.Application.Dtos.Users;
using ConcertStats.Core.Entities;

namespace ConcertStats.Application.Dtos.Mapper.Users;

public static class UserDtoMapper
{
    public static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Credentials.Email,
            LastLoginAt = user.Credentials.LastSuccessfulLogin,
            ProfileFullName = user.Profile.FullName,
            ProfilePictureUrl = user.Profile.ProfilePictureUrl ?? string.Empty,
            ProfileBio = user.Profile.Bio ?? string.Empty,
            ProfileLocation = user.Profile.Location ?? string.Empty,
            LanguageSettings = user.Settings.Language.ToString(),
            ThemeSettings = user.Settings.Theme.ToString(),
            PrivacySettings = user.Settings.PrivacySettings.ToString()
        };
    }
}

        
        
