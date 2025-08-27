namespace ConcertStats.Application.Dtos.Request.Venues;

public class UpdateVenueRequest
{
    public string Name { get; set; } = string.Empty;
    public int? Capacity { get; set; }
}