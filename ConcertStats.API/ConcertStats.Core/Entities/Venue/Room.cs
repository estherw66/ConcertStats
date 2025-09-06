namespace ConcertStats.Core.Entities;

public class Room
{
    public int Id { get; set; }
    public string? RoomName { get; set; }
    public int? Capacity { get; set; }
    
    public int VenueId { get; set; }
    public Venue Venue { get; set; } = null!;
}