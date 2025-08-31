using ConcertStats.Application.Dtos.Request.Users;
using ConcertStats.Core.Entities;
using ConcertStats.Core.Entities.User;
using ConcertStats.Core.Enums;

namespace ConcertStats.Application.Dtos.Mapper.Users;

public static class CreateUserRequestMapper
{
    public static User ToEntity(CreateUserRequest request)
    {
        return new User
        {
            Username = request.Username,
            Credentials = new UserCredentials{},
            Profile = new UserProfile
            {
                FullName = request.FullName
            },
            Settings = new UserSettings{},
            Roles = []
        };
    }
}