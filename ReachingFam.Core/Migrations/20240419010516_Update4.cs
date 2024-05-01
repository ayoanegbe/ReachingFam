using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReachingFam.Core.Migrations
{
    /// <inheritdoc />
    public partial class Update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adults",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "Children",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "FamilySize",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "Seniors",
                table: "Families");

            migrationBuilder.AddColumn<int>(
                name: "Adults",
                table: "Hampers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Children",
                table: "Hampers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FamilySize",
                table: "Hampers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Seniors",
                table: "Hampers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adults",
                table: "Hampers");

            migrationBuilder.DropColumn(
                name: "Children",
                table: "Hampers");

            migrationBuilder.DropColumn(
                name: "FamilySize",
                table: "Hampers");

            migrationBuilder.DropColumn(
                name: "Seniors",
                table: "Hampers");

            migrationBuilder.AddColumn<int>(
                name: "Adults",
                table: "Families",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Children",
                table: "Families",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FamilySize",
                table: "Families",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Seniors",
                table: "Families",
                type: "int",
                nullable: true);
        }
    }
}
