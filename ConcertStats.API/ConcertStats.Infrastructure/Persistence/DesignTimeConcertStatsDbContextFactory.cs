using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace ConcertStats.Infrastructure.Persistence;

internal class DesignTimeConcertStatsDbContextFactory
    : IDesignTimeDbContextFactory<ConcertStatsDbContext>
{

    public ConcertStatsDbContext CreateDbContext(string[] args)
    {
        DotNetEnv.Env.Load();
        var connectionString = Environment.GetEnvironmentVariable("CONCERT_STATS_CONNECTION_STRING") ??
                               throw new InvalidOperationException("DefaultConnection not found");

        const string migrationsHistoryTable = "_ConcertStatsMigrationsHistory";
        const string migrationsHistorySchema = "concert_stats";
        
        var optionsBuilder = new DbContextOptionsBuilder<ConcertStatsDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                sqlOptions => sqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore));
        
        return new ConcertStatsDbContext(optionsBuilder.Options);
    }
}