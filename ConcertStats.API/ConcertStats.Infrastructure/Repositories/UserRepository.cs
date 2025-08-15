using ConcertStats.Application.Interfaces.Repositories;
using ConcertStats.Core.Entities;
using ConcertStats.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ConcertStats.Infrastructure.Repositories;

public class UserRepository(ConcertStatsDbContext context) : IUserRepository
{
    public async Task CreateAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<User>> GetAllAsync(int skip, int pageSize, string searchQuery)
    {
        var query = context.Users.AsQueryable();
        
        if (!string.IsNullOrEmpty(searchQuery))
        {
            var likeQuery = $"%{searchQuery}%";
            query = query.Where(u => EF.Functions.Like(u.Username, likeQuery) ||
                                     EF.Functions.Like(u.Profile.FullName, likeQuery));
        }
        
        var users = await query
            .Include(u => u.Profile)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
        
        return users;
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        return await context.Users
            .Include(u => u.Profile)
            .Include(u => u.Credentials)
            .Include(u => u.Settings)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await context.Users
            .Include(u => u.Profile)
            .Include(u => u.Roles)
            .Include(u => u.Settings)
            .Include(u => u.Credentials)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users
            .Include(u => u.Credentials)
            .Include(u => u.Roles)
            .Include(u => u.Profile)
            .Include(u => u.Settings)
            .FirstOrDefaultAsync(u => u.Credentials.EmailHash == email);
    }

    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int userId)
    {
        var user = await context.Users.FindAsync(userId);

        if (user == null)
        {
            return false;
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }
}