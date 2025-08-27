using ConcertStats.Core.Entities;

namespace ConcertStats.Application.Interfaces.Repositories;

public interface IVenueRepository
{
    Task CreateAsync(Venue venue);
    Task<ICollection<Venue>> GetAllByCityAsync(string city);
    Task<ICollection<Venue>> GetAllByCountryAsync(string country);
    Task<Venue?> GetByIdAsync(int venueId);
    Task<Venue?> GetByNameAsync(string name);
    Task UpdateAsync(Venue venue);
    Task<bool> DeleteAsync(int venueId);
    Task<bool> VenueExistsAsync(string name, string city, string country);
}