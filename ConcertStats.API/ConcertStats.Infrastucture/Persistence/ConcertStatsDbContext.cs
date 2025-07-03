using ConcertStats.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConcertStats.Infrastucture.Persistence;

public class ConcertStatsDbContext : DbContext
{
    public ConcertStatsDbContext(DbContextOptions<ConcertStatsDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserConcert> UserConcerts { get; set; }
    public DbSet<UserCredentials> UserCredentials { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }

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
    }
}