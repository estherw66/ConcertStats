using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConcertStats.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsLockedOutcollumntousercredentials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLockedOut",
                table: "UserCredentials",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLockedOut",
                table: "UserCredentials");
        }
    }
}
