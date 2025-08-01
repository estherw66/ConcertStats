namespace ConcertStats.Application.Dtos.Request.Users;

public class UpdateUserSettingsRequest
{
    public string Language { get; set; } = string.Empty;
    public string Theme { get; set; } = string.Empty;
    public string PrivacySettings { get; set; } = string.Empty;
}