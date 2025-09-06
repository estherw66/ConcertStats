using ConcertStats.Application.Dtos.Artists;
using ConcertStats.Core.Entities;

namespace ConcertStats.Application.Interfaces.Repositories;

public interface IArtistRepository
{
    Task CreateAsync(Artist artist);
    Task<ICollection<Artist>> GetAllAsync(int skip, int pageSize, string searchQuery);
    Task<Artist?> GetByIdAsync(int artistId);
    Task<Artist?> GetByNameAsync(string name);
    Task UpdateAsync(Artist artist);
    Task<bool> DeleteAsync(int artistId);
    Task<bool> ExistsAsync(int id, string artistName);
}