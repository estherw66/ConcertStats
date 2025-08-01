using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConcertStats.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Addemailhash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailHash",
                table: "UserCredentials",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailHash",
                table: "UserCredentials");
        }
    }
}
