using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadFermaHub.Migrations
{
    /// <inheritdoc />
    public partial class AddEndPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Endpoints",
                table: "Microservices",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Endpoints",
                table: "Microservices");
        }
    }
}
