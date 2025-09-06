namespace ConcertStats.Application.Dtos.Request.Artists;

public class CreateArtistRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}