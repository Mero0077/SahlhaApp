using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SahlhaApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editjob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Jobs");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Jobs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Jobs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Jobs");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
