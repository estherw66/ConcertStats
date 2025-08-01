using ConcertStats.Core.Enums;

namespace ConcertStats.Core.Entities;

public class UserRoleJoin
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public UserRole UserRole { get; set; }
}