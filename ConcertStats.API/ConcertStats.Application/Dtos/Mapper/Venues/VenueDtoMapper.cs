using ConcertStats.Application.Dtos.Venues;
using ConcertStats.Core.Entities;

namespace ConcertStats.Application.Dtos.Mapper.Venues;

public static class VenueDtoMapper
{
    public static VenueDto ToDto(Venue venue)
    {
        return new VenueDto
        {
            Id = venue.Id,
            Name = venue.Name,
            City = venue.City,
            Country = venue.Country,
            Capacity = venue.Capacity ?? 0
        };
    }
}