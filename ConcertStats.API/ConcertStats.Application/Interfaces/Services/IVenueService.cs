using ConcertStats.Application.Dtos.Request.Venues;
using ConcertStats.Application.Dtos.Venues;

namespace ConcertStats.Application.Interfaces.Services;

public interface IVenueService
{
    Task<VenueDto> CreateVenueAsync(CreateVenueRequest request);
    Task<VenueDto> GetVenueByIdAsync(int venueId);
    Task<ICollection<VenueDto>> GetAllVenuesByCityAsync(string city);
    Task<ICollection<VenueDto>> GetAllVenuesByCountryAsync(string country);
    Task<VenueDto> UpdateVenueAsync(int venueId, UpdateVenueRequest request);
    Task<bool> DeleteVenueAsync(int venueId);
}