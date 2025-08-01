namespace ConcertStats.Core.Entities;

public class UserProfile
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string? Location { get; set; }

    public User? User { get; set; }
    public int UserId { get; set; }
}