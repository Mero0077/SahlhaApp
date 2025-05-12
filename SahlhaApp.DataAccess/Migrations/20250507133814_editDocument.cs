using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SahlhaApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DocumentTypes_DocumentTypeId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Providers_ProviderId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_DocumentTypeId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_ProviderId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Documents");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ApplicationUserId",
                table: "Documents",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_AspNetUsers_ApplicationUserId",
                table: "Documents",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_AspNetUsers_ApplicationUserId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_ApplicationUserId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Documents");

            migrationBuilder.AddColumn<int>(
                name: "DocumentTypeId",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProviderId",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentTypeId",
                table: "Documents",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ProviderId",
                table: "Documents",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DocumentTypes_DocumentTypeId",
                table: "Documents",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Providers_ProviderId",
                table: "Documents",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
