using ConcertStats.Core.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace ConcertStats.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task CreateAsync(User user);
    Task<ICollection<User>> GetAllAsync(int skip, int pageSize, string searchQuery);
    Task<User?> GetByIdAsync(int userId);
    Task<User?> GetByUsernameAsync(string  username);
    Task<User?> GetByEmailAsync(string  email);
    Task UpdateAsync(User user);
    Task<bool> DeleteAsync(int userId);
}   