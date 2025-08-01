using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConcertStats.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removepasswordsalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "UserCredentials");

            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "UserCredentials",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "UserCredentials");

            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "UserCredentials",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
