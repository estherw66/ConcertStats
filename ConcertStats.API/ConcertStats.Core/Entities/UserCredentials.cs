namespace ConcertStats.Core.Entities;

public class UserCredentials
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Retry { get; set; }
    public DateTime? LastSuccessfulLogin { get; set; }
    public DateTime? LastFailedLogin { get; set; }
    public DateTime? LockoutEnd { get; set; }

    public User? User { get; set; }
    public int UserId { get; set; }
}