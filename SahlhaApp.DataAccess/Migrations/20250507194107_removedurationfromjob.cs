using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SahlhaApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removedurationfromjob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Jobs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Duration",
                table: "Jobs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
