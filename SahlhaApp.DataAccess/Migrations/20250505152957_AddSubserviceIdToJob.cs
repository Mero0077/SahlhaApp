using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SahlhaApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSubserviceIdToJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubServiceId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubServiceId",
                table: "Jobs");
        }
    }
}
