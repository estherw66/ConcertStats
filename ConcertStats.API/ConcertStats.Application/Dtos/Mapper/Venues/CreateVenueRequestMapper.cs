using ConcertStats.Application.Dtos.Request.Venues;
using ConcertStats.Core.Entities;

namespace ConcertStats.Application.Dtos.Mapper.Venues;

public static class CreateVenueRequestMapper
{
    public static Venue ToEntity(CreateVenueRequest request)
    {
        return new Venue
        {
            Name = request.Name,
            City = request.City,
            Country = request.Country
        };
    }
}