using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReachingFam.Core.Migrations
{
    /// <inheritdoc />
    public partial class Update7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailPath",
                table: "PartnerGiveOuts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailPath",
                table: "InwardItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Hampers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailPath",
                table: "Hampers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailPath",
                table: "PartnerGiveOuts");

            migrationBuilder.DropColumn(
                name: "ThumbnailPath",
                table: "InwardItems");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Hampers");

            migrationBuilder.DropColumn(
                name: "ThumbnailPath",
                table: "Hampers");
        }
    }
}
