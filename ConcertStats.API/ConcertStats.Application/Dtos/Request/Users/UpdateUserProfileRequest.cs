namespace ConcertStats.Application.Dtos.Request.Users;

public class UpdateUserProfileRequest
{
    public string FullName { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string? Location { get; set; }
}