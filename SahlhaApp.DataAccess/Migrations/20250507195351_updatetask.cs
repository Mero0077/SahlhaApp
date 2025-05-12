using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SahlhaApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatetask : Migration
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

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_SubServiceId",
                table: "Jobs",
                column: "SubServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_SubServices_SubServiceId",
                table: "Jobs",
                column: "SubServiceId",
                principalTable: "SubServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_SubServices_SubServiceId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_SubServiceId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "SubServiceId",
                table: "Jobs");
        }
    }
}
