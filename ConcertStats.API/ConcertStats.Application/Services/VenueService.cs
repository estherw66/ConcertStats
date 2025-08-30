using ConcertStats.Application.Dtos.Mapper.Venues;
using ConcertStats.Application.Dtos.Request.Venues;
using ConcertStats.Application.Dtos.Venues;
using ConcertStats.Application.Interfaces.Repositories;
using ConcertStats.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ConcertStats.Application.Services;

public class VenueService(
    IVenueRepository venueRepository,
    ILogger<VenueService> logger) 
    : IVenueService
{
    public async Task<VenueDto> CreateVenueAsync(CreateVenueRequest request)
    {
        // todo check room for whitespace
        if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.City) || string.IsNullOrEmpty(request.Country))
        {
            throw new ArgumentException("Please fill in all required fields.");
        }
        
        var venue =
            await venueRepository.GetByNameCityCountryAsync(request.Name, request.City, request.Country);
        
        if (venue == null)
        {
            var newVenue = CreateVenueRequestMapper.ToEntity(request);
            if (!string.IsNullOrEmpty(request.RoomName))
            {
                // newVenue.Rooms.Add(request.RoomName);
            }
            
            await venueRepository.CreateAsync(newVenue);
            return VenueDtoMapper.ToDto(newVenue);
        }
        
        if (string.IsNullOrEmpty(request.RoomName)) return VenueDtoMapper.ToDto(venue);
        // var roomExists = venue.Rooms.Any(r => r.Equals(request.RoomName, StringComparison.OrdinalIgnoreCase));
        // if (roomExists)
        // {
        //     throw new InvalidOperationException("Room already exists for this venue.");
        // }
            
        // venue.Rooms.Add(request.RoomName);
        await venueRepository.UpdateAsync(venue);

        return VenueDtoMapper.ToDto(venue);
    }

    public async Task<VenueDto> GetVenueByIdAsync(int venueId)
    {
        var venue = await venueRepository.GetByIdAsync(venueId);
        if (venue == null)
        {
            throw new InvalidOperationException("Venue not found.");
        }
        
        var venueDto = VenueDtoMapper.ToDto(venue);
        return venueDto;
    }

    public async Task<ICollection<VenueDto>> GetAllVenuesByCityAsync(string city)
    {
        var venues = await venueRepository.GetAllByCityAsync(city);
        var venueDtos = venues.Select(VenueDtoMapper.ToDto).ToList();
        return venueDtos;
    }

    public async Task<ICollection<VenueDto>> GetAllVenuesByCountryAsync(string country)
    {
        var venues = await venueRepository.GetAllByCountryAsync(country);
        var venueDtos = venues.Select(VenueDtoMapper.ToDto).ToList();
        return venueDtos;
    }

    public async Task<VenueDto> UpdateVenueAsync(int venueId, UpdateVenueRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            throw new ArgumentException("Please fill in all required fields.");
        }
        
        var venue = await venueRepository.GetByIdAsync(venueId);
        if (venue == null)
        {
            throw new InvalidOperationException("Venue not found.");
        }
        
        // check if venue with same name exists
        if (!await venueRepository.VenueExistsAsync(request.Name, venue.City, venue.Country))
        {
            venue.Name = request.Name;
        }
        
        // check capacity
        if (request.Capacity is > 0)
        {
            venue.Capacity = request.Capacity;
        }
        
        await venueRepository.UpdateAsync(venue);
        var venueDto = VenueDtoMapper.ToDto(venue);
        return venueDto;
    }

    public async Task<bool> DeleteVenueAsync(int venueId)
    {
        var venue = await venueRepository.GetByIdAsync(venueId);
        if (venue == null)
        {
            throw new InvalidOperationException("Venue not found.");
        }

        return await venueRepository.DeleteAsync(venueId);
    }
}