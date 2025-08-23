using ConcertStats.Core.Enums;

namespace ConcertStats.Core.Entities;

public class UserSettings
{
    public UserSettings()
    {
        Language = Language.English;
        Theme = Theme.SystemDefault;
        PrivacySettings = PrivacySettings.Public;
        TwoFactorEnabled = false;
    }
    
    public int Id { get; set; }
    public string? TimeZone { get; set; }
    public Language Language { get; set; }
    public Theme Theme { get; set; }
    public PrivacySettings PrivacySettings { get; set; }
    public bool TwoFactorEnabled { get; set; }

    public User.User? User { get; set; }
    public int UserId { get; set; }
}