using ConcertStats.Application.Dtos.Users;
using ConcertStats.Core.Entities;
using ConcertStats.Core.Entities.User;

namespace ConcertStats.Application.Dtos.Mapper.Users;

public static class UserResultMapper
{
    public static UserResultDto ToDto(User user)
    {
        return new UserResultDto
        {
            ProfilePictureUrl = user.Profile.ProfilePictureUrl ?? string.Empty,
            Username = user.Username,
            FullName = user.Profile.FullName
        };
    }
}