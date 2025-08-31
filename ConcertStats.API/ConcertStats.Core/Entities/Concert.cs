using ConcertStats.Core.Enums;

namespace ConcertStats.Core.Entities;

public class Concert
{
    public int Id { get; set; }

    public ICollection<ConcertArtist> Artists { get; init; } = [];

    // todo add to dto
    // public ICollection<Artist> Headliners { get; init; } = [];
    // public ICollection<Artist> Supports { get; init; } = [];
    
    public string? Tour { get; set; }
    public Venue Venue { get; set; } = null!;
    public EventType EventType { get; set; }
    public bool IsCancelled { get; set; }
    public bool IsRescheduled { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}