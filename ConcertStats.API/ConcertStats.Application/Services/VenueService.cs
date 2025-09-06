using ConcertStats.Application.Dtos.Mapper.Venues;
using ConcertStats.Application.Dtos.Request.Venues;
using ConcertStats.Application.Dtos.Venues;
using ConcertStats.Application.Interfaces.Repositories;
using ConcertStats.Application.Interfaces.Services;
using ConcertStats.Core.Entities;
using Microsoft.Extensions.Logging;

namespace ConcertStats.Application.Services;

public class VenueService(
    IVenueRepository venueRepository,
    ILogger<VenueService> logger)
    : IVenueService
{
    public async Task<VenueDto> CreateVenueAsync(CreateVenueRequest request)
    {
        ValidateCreateVenueRequest(request);

        // check if venue already exists
        var venue =
            await venueRepository.GetByNameCityCountryAsync(request.Name, request.City, request.Country);

        // venue exists
        if (venue != null)
        {
            return await AddRoomToExistingVenue(venue, request.RoomName!, request.RoomCapacity);
        }

        // venue does not exist, create new
        var newVenue = CreateNewVenue(request);
        await venueRepository.CreateAsync(newVenue);

        return VenueDtoMapper.ToDto(newVenue);
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

    public async Task<VenueDto> GetVenueByNameAsync(string name)
    {
        var venue = await venueRepository.GetByNameAsync(name);
        if (venue == null)
        {
            throw new InvalidOperationException("Venue not found.");
        }

        return VenueDtoMapper.ToDto(venue);
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
        // todo add helper methods
        if (string.IsNullOrEmpty(request.Name)) throw new ArgumentException("Please fill in all required fields.");

        var venue = await venueRepository.GetByIdAsync(venueId);
        if (venue == null) throw new NullReferenceException("Venue not found.");
  
        // normalise inputs
        var newVenueName = request.Name.Trim();
        var newRoomName = string.IsNullOrWhiteSpace(request.RoomName) ? null : request.RoomName.Trim();
        var capacity = request.Capacity < 0 ? 0 : request.Capacity;
        if (capacity > 1000000)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be between 0 and 1000000.");
        
        // update venue name if changed
        if (!venue.Name.Equals(newVenueName, StringComparison.OrdinalIgnoreCase))
        {
            // check if venue with same name exists
            if (await venueRepository.VenueExistsAsync(newVenueName, venue.City, venue.Country))
                throw new InvalidOperationException("Venue with this name already exists."); 
            venue.Name = newVenueName;
        }
        
        // check if room exists
        if (newRoomName != null)
        {
            var room = venue.Rooms
                .FirstOrDefault(r => r.RoomName != null &&
                                     r.RoomName.Equals(newRoomName, StringComparison.OrdinalIgnoreCase));

            if (room != null)
            {
                room.Capacity = capacity;
            }
            else
            {
                var newRoom = CreateRoom(newRoomName, capacity, venue);
                venue.Rooms.Add(newRoom);
            }
        }
        
        venue.LastUpdated = DateTime.UtcNow;
        
        await venueRepository.UpdateAsync(venue);
        return VenueDtoMapper.ToDto(venue);
    }

    public async Task<bool> DeleteVenueAsync(int venueId)
    {
        var venue = await venueRepository.GetByIdAsync(venueId);
        if (venue == null)
        {
            throw new NullReferenceException("Venue not found.");
        }

        return await venueRepository.DeleteAsync(venueId);
    }

    private void ValidateCreateVenueRequest(CreateVenueRequest request)
    {
        if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.City) ||
            string.IsNullOrEmpty(request.Country))
        {
            throw new ArgumentException("Please fill in all required fields.");
        }

        if (string.IsNullOrEmpty(request.RoomName))
        {
            request.RoomName = "Main";
        }
        
        if (request.RoomCapacity is < 0)
        {
            request.RoomCapacity = 0;
        }
        
        if (request.RoomCapacity is > 1000000)
            throw new ArgumentOutOfRangeException
                (nameof(request.RoomCapacity), "Capacity must be between 0 and 1000000.");
    }

    private bool RoomExists(Venue venue, string roomName)
    {
        return venue.Rooms
            .Any(r => r.RoomName != null
                      && r.RoomName.Equals(roomName, StringComparison.OrdinalIgnoreCase));
    }

    private Room CreateRoom(string roomName, int? capacity, Venue venue)
    {
        var room = new Room
        {
            RoomName = roomName,
            Capacity = capacity ?? 0,
            VenueId = venue.Id,
            Venue = venue
        };
        return room;
    }

    private async Task<VenueDto> AddRoomToExistingVenue(Venue venue, string roomName, int? capacity)
    {
        if (RoomExists(venue, roomName)) 
            throw new InvalidOperationException($"Room with name {roomName} already exists.");
        
        var newRoom = CreateRoom(roomName, capacity, venue);
        venue.Rooms.Add(newRoom);
        venue.LastUpdated = DateTime.UtcNow;
        await venueRepository.UpdateAsync(venue);
        
        return VenueDtoMapper.ToDto(venue);
    }
    
    private Venue CreateNewVenue(CreateVenueRequest request)
    {
        var newVenue = new Venue
        {
            Name = request.Name,
            City = request.City,
            Country = request.Country,
            Rooms = []
        };
        newVenue.Rooms.Add(CreateRoom(request.RoomName!, request.RoomCapacity, newVenue));
        
        return newVenue;
    }
}