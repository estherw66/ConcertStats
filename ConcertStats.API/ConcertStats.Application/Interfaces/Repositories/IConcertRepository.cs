using ConcertStats.Core.Entities;

namespace ConcertStats.Application.Interfaces.Repositories;

public interface IConcertRepository
{
    Task CreateAsync(Concert concert);
    Task<ICollection<Concert>> GetAllAsync(int skip, int pageSize, string searchQuery);
    Task<ICollection<Concert>> GetAllByUser(int userId);
    Task<Concert?> GetByIdAsync(int concertId);
    Task UpdateAsync(Concert concert);
    Task<bool> DeleteAsync(int concertId);
}