using ConcertStats.Core.Entities;
using ConcertStats.Core.Entities.User;
using ConcertStats.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace ConcertStats.Infrastructure.Persistence;

public class ConcertStatsDbContext : DbContext
{
    public ConcertStatsDbContext(DbContextOptions<ConcertStatsDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserCredentials> UserCredentials { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }
    public DbSet<UserRoleJoin> UserRoles { get; set; }

    public DbSet<Artist> Artists { get; set; }  
    public DbSet<Concert> Concerts { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Room> Rooms { get; set; }
    
    public DbSet<ConcertArtist> ConcertArtists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId);
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Credentials)
            .WithOne(c => c.User)
            .HasForeignKey<UserCredentials>(c => c.UserId);
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Settings)
            .WithOne(s => s.User)
            .HasForeignKey<UserSettings>(s => s.UserId);
        
        modelBuilder.Entity<UserRoleJoin>()
            .HasKey(ur => new { ur.UserId, ur.UserRole });
        
        modelBuilder.Entity<UserRoleJoin>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.Roles)
            .HasForeignKey(ur => ur.UserId);
        
        modelBuilder.Entity<ConcertArtist>()
            .HasKey(ca => new { ca.ConcertId, ca.ArtistId });
        
        modelBuilder.Entity<ConcertArtist>()
            .HasOne(ca => ca.Concert)
            .WithMany(c => c.Artists)
            .HasForeignKey(ca => ca.ConcertId);
        
        modelBuilder.Entity<ConcertArtist>()
            .HasOne(ca => ca.Artist)
            .WithMany(a => a.Concerts)
            .HasForeignKey(ca => ca.ArtistId);

        modelBuilder.Entity<Venue>()
            .HasMany(v => v.Rooms);
        
        modelBuilder.Entity<Room>()
            .HasOne(r => r.Venue)
            .WithMany(v => v.Rooms)
            .HasForeignKey(r => r.VenueId);
    }
}