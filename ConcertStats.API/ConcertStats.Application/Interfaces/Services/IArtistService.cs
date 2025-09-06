using ConcertStats.Application.Dtos.Artists;
using ConcertStats.Application.Dtos.Request.Artists;

namespace ConcertStats.Application.Interfaces.Services;

public interface IArtistService
{
    Task<ArtistDto> CreateArtistAsync(CreateArtistRequest request);
    Task<IEnumerable<ArtistDto>> GetAllArtistsAsync(int skip, int pageSize, string searchQuery);
    Task<ArtistDto> GetArtistByIdAsync(int id);
    Task<ArtistDto> GetArtistByNameAsync(string name);
    Task UpdateArtistAsync(int id, UpdateArtistRequest request);
    Task<bool> DeleteArtistAsync(int id);
}