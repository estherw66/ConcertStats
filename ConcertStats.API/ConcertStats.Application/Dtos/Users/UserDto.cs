namespace ConcertStats.Application.Dtos.Users;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime? LastLoginAt { get; set; }
    
    public string ProfileFullName { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public string? ProfileBio { get; set; }
    public string? ProfileLocation { get; set; }
    
    public string LanguageSettings { get; set; } = string.Empty;
    public string ThemeSettings { get; set; } = string.Empty;
    public string PrivacySettings { get; set; } = string.Empty;
}