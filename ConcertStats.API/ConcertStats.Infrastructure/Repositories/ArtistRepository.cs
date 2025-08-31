using ConcertStats.Application.Interfaces.Repositories;
using ConcertStats.Core.Entities;
using ConcertStats.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ConcertStats.Infrastructure.Repositories;

public class ArtistRepository(ConcertStatsDbContext context) : IArtistRepository
{
    public async Task CreateAsync(Artist artist)
    {
        context.Artists.Add(artist);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<Artist>> GetAllAsync(int skip, int pageSize, string searchQuery)
    {
        var query = context.Artists.AsQueryable();
        if (!string.IsNullOrEmpty(searchQuery))
        {
            var likeQuery = $"%{searchQuery}%";
            query = query.Where(a => EF.Functions.Like(a.Name, likeQuery));
        }
        
        var artists = await query
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
        
        return artists;
    }

    public async Task<Artist?> GetByIdAsync(int artistId)
    {
        return await context.Artists
            .FirstOrDefaultAsync(a => a.Id == artistId);
    }

    public async Task<Artist?> GetByNameAsync(string name)
    {
        return await context.Artists
            .FirstOrDefaultAsync(a => a.Name == name);
    }

    public async Task UpdateAsync(Artist artist)
    {
        context.Artists.Update(artist);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int artistId)
    {
        var artist = await context.Artists.FindAsync(artistId);
        
        if (artist == null)
        {
            return false;
        }
        
        context.Artists.Remove(artist);
        await context.SaveChangesAsync();
        return true;
    }
}