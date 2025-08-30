using ConcertStats.Application.Interfaces.Repositories;
using ConcertStats.Core.Entities;
using ConcertStats.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ConcertStats.Infrastructure.Repositories;

public class VenueRepository(ConcertStatsDbContext context) : IVenueRepository
{
    public async Task CreateAsync(Venue venue)
    {
        context.Venues.Add(venue);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<Venue>> GetAllByCityAsync(string city)
    {
        var venues = await context.Venues
            .Where(v => v.City == city)
            .ToListAsync();
        return venues;
    }

    public async Task<ICollection<Venue>> GetAllByCountryAsync(string country)
    {
        var venues = await context.Venues
            .Where(v => v.Country == country)
            .ToListAsync();
        return venues;
    }

    public async Task<Venue?> GetByIdAsync(int venueId)
    {
        return await context.Venues
            .FirstOrDefaultAsync(v => v.Id == venueId);
    }

    public async Task<Venue?> GetByNameAsync(string name)
    {
        return await context.Venues
            .FirstOrDefaultAsync(v => v.Name == name);
    }

    public async Task<Venue?> GetByNameCityCountryAsync(string name, string city, string country)
    {
        return await context.Venues
            .FirstOrDefaultAsync(v => v.Name == name 
                                      && v.City == city 
                                      && v.Country == country);
    }

    public async Task UpdateAsync(Venue venue)
    {
        context.Venues.Update(venue);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int venueId)
    {
        var venue = await context.Venues.FindAsync(venueId);
        
        if (venue == null)
        {
            return false;
        }
        
        context.Venues.Remove(venue);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> VenueExistsAsync(string name, string city, string country)
    {
        var venue = await context.Venues
            .FirstOrDefaultAsync(v => v.Name == name 
                                      && v.City == city 
                                      && v.Country == country);
        
        return venue != null;
    }
}