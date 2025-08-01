using ConcertStats.Core.Entities;
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
    }
}