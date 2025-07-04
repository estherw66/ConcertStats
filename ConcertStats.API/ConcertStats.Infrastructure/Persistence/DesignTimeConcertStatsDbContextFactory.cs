using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace ConcertStats.Infrastructure.Persistence;

internal class DesignTimeConcertStatsDbContextFactory : IDesignTimeDbContextFactory<ConcertStatsDbContext>
{
    public ConcertStatsDbContext CreateDbContext(string[] args)
    {
        const string connectionString = "Server=localhost;Port=3306;Database=concert_stats;Uid=root;Pwd=CountingSheep;";
        const string migrationsHistoryTable = "_ConcertStatsMigrationsHistory";
        const string migrationsHistorySchema = "concert_stats";
        
        var optionsBuilder = new DbContextOptionsBuilder<ConcertStatsDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                options => options.SchemaBehavior(MySqlSchemaBehavior.Ignore));
        
        return new ConcertStatsDbContext(optionsBuilder.Options);
    }
}