namespace ConcertStats.Core.Entities;

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public ICollection<ConcertArtist> Concerts { get; set; } = [];
}