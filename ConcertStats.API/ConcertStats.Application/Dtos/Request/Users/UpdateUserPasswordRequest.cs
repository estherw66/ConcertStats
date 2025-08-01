namespace ConcertStats.Application.Dtos.Request.Users;

public class UpdateUserPasswordRequest
{
    public string OldPassword { get; set; }  = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}