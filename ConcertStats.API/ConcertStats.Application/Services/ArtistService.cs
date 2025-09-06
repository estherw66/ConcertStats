using System.Globalization;
using System.Text.RegularExpressions;
using ConcertStats.Application.Dtos.Artists;
using ConcertStats.Application.Dtos.Mapper.Artists;
using ConcertStats.Application.Dtos.Request.Artists;
using ConcertStats.Application.Exceptions;
using ConcertStats.Application.Interfaces.Repositories;
using ConcertStats.Application.Interfaces.Services;
using ConcertStats.Core.Entities;

namespace ConcertStats.Application.Services;

public class ArtistService(IArtistRepository artistRepository) : IArtistService
{
    public async Task<ArtistDto> CreateArtistAsync(CreateArtistRequest request)
    {
        var artistName = ValidateAristName(request.Name);

        var artist = await artistRepository.GetByNameAsync(artistName);

        if (artist != null)
        {
            throw new InvalidOperationException($"Artist with name '{artistName}' already exists.");
        }

        var newArtist = new Artist
        {
            Name = artistName,
            Description = request.Description
        };

        await artistRepository.CreateAsync(newArtist);

        return ArtistDtoMapper.ToDto(newArtist);
    }

    public async Task<IEnumerable<ArtistDto>> GetAllArtistsAsync(int pageNumber, int pageSize, string searchQuery)
    {
        var skip = (pageNumber - 1) * pageSize;

        var artists = await artistRepository.GetAllAsync(skip, pageSize, searchQuery);
        return artists.Select(ArtistDtoMapper.ToDto);
    }

    public async Task<ArtistDto> GetArtistByIdAsync(int id)
    {
        var artist = await artistRepository.GetByIdAsync(id);
        return artist == null ? throw new ArtistNotFoundException(id) : ArtistDtoMapper.ToDto(artist);
    }

    public async Task<ArtistDto> GetArtistByNameAsync(string name)
    {
        var artistName = ValidateAristName(name);
        var artist = await artistRepository.GetByNameAsync(artistName);
        return artist == null ? throw new ArtistNotFoundException(name) : ArtistDtoMapper.ToDto(artist);
    }

    public async Task UpdateArtistAsync(int id, UpdateArtistRequest request)
    {
        var artistName = ValidateAristName(request.Name);

        var artist = await artistRepository.GetByIdAsync(id);
        if (artist == null)
        {
            throw new ArtistNotFoundException(id);
        }

        if (await artistRepository.ExistsAsync(id, artistName))
        {
            throw new InvalidOperationException($"Artist with name '{artistName}' already exists.");
        }

        artist.Name = artistName;

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            artist.Description = ValidateDescription(request.Description);
        }

        await artistRepository.UpdateAsync(artist);
    }

    public async Task<bool> DeleteArtistAsync(int id)
    {
        var artist = await artistRepository.GetByIdAsync(id);
        if (artist == null)
        {
            throw new ArtistNotFoundException(id);
        }
        
        return await artistRepository.DeleteAsync(id);
    }

    private string ValidateAristName(string artistName)
    {
        if (string.IsNullOrWhiteSpace(artistName))
        {
            throw new ArgumentException("Artist name cannot be null or empty.");
        }

        if (artistName.Length > 100)
        {
            throw new ArgumentException("Artist name cannot exceed 100 characters.");
        }

        var validatedName = artistName.Trim();
        validatedName = validatedName.ToLower();
        validatedName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(validatedName);

        return validatedName;
    }

    private string ValidateDescription(string description)
    {
        if (description is { Length: > 1000 })
        {
            throw new ArgumentException("Artist description cannot exceed 1000 characters.");
        }

        var validatedDescription = Regex.Replace(
            description.Trim(),
            @"(^[a-z])|(?<=[\.!\?]\s)[a-z]",
            m => m.Value.ToUpper()
        );

        return validatedDescription;
    }
}