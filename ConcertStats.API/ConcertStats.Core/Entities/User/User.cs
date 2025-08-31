namespace ConcertStats.Core.Entities.User;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public ICollection<UserRoleJoin> Roles { get; set; } = [];

    public UserProfile Profile { get; set; } = new();
    public UserCredentials Credentials { get; set; } = new();
    public UserSettings Settings { get; set; } = new();
    // public ICollection<UserConcert> ConcertsVisited { get; set; } = [];
}
