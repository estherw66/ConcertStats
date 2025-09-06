using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConcertStats.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Adddescriptiontoartist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Artists",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Artists");
        }
    }
}
