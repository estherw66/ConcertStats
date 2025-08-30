namespace ConcertStats.Application.Dtos.Request.Venues;

public class CreateVenueRequest
{
    public string Name { get; set; } = string.Empty;
    public string? RoomName { get; set; }
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int? Capacity { get; set; }
}