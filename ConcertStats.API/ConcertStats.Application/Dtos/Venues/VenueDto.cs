namespace ConcertStats.Application.Dtos.Venues;

public class VenueDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public ICollection<RoomDto> Rooms { get; set; } = [];
}

public class RoomDto
{
    public string RoomName { get; set; } = string.Empty;
    public int? Capacity { get; set; }
}