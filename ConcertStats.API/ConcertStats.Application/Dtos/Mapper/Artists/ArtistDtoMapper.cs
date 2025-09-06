using ConcertStats.Application.Dtos.Artists;
using ConcertStats.Core.Entities;

namespace ConcertStats.Application.Dtos.Mapper.Artists;

public static class ArtistDtoMapper
{
    public static ArtistDto ToDto(Artist artist)
    {
        return new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            Description = artist.Description
        };
    }
}