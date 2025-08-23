using ConcertStats.Core.Enums;

namespace ConcertStats.Core.Entities;

public class ConcertArtist
{
    public int ConcertId { get; set; }
    public Concert Concert { get; private set; } = null!;

    public int ArtistId { get; set; }
    public Artist Artist { get; private set; } = null!;

    public ArtistRole Role { get; set; }
}