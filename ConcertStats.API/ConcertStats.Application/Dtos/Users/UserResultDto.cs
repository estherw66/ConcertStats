namespace ConcertStats.Application.Dtos.Users;

public class UserResultDto
{
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
}