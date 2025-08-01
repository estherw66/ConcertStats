namespace ConcertStats.Application.Dtos.Users;

public class UserProfileDto
{
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string? Location { get; set; }
    public string PrivacySettings { get; set; } = string.Empty;
}