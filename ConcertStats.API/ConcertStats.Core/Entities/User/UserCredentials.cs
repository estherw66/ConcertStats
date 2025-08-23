namespace ConcertStats.Core.Entities;

public class UserCredentials
{
    public UserCredentials()
    {
        EmailConfirmed = false;
        IsActive = true;
        Retry = 0;
        IsLockedOut = false;
    }
    
    public int Id { get; init; }
    public string Email { get; set; } = string.Empty;
    public string EmailHash { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Retry { get; set; }
    public bool IsLockedOut { get; set; }
    public DateTime? LastSuccessfulLogin { get; set; }
    public DateTime? LastFailedLogin { get; set; }
    public DateTime? LockoutEnd { get; set; }

    public User.User? User { get; set; }
    public int UserId { get; set; }
}

