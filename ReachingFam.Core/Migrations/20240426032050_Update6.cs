using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReachingFam.Core.Migrations
{
    /// <inheritdoc />
    public partial class Update6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Frozen",
                table: "Wastes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "FrozenWeight",
                table: "Wastes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NonFood",
                table: "Wastes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "NonFoodWeight",
                table: "Wastes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NonPerishables",
                table: "Wastes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "NonPerishablesWeight",
                table: "Wastes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Perishables",
                table: "Wastes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PerishablesWeight",
                table: "Wastes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Frozen",
                table: "VolunteerGiveOuts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "FrozenWeight",
                table: "VolunteerGiveOuts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NonFood",
                table: "VolunteerGiveOuts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "NonFoodWeight",
                table: "VolunteerGiveOuts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NonPerishables",
                table: "VolunteerGiveOuts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "NonPerishablesWeight",
                table: "VolunteerGiveOuts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Perishables",
                table: "VolunteerGiveOuts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PerishablesWeight",
                table: "VolunteerGiveOuts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Adults",
                table: "PartnerGiveOuts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Children",
                table: "PartnerGiveOuts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "PartnerGiveOuts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Frozen",
                table: "PartnerGiveOuts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "FrozenWeight",
                table: "PartnerGiveOuts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NonFood",
                table: "PartnerGiveOuts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "NonFoodWeight",
                table: "PartnerGiveOuts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NonPerishables",
                table: "PartnerGiveOuts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "NonPerishablesWeight",
                table: "PartnerGiveOuts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfFamilies",
                table: "PartnerGiveOuts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Perishables",
                table: "PartnerGiveOuts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PerishablesWeight",
                table: "PartnerGiveOuts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Seniors",
                table: "PartnerGiveOuts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "InwardItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Frozen",
                table: "InwardItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "FrozenWeight",
                table: "InwardItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NonFood",
                table: "InwardItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "NonFoodWeight",
                table: "InwardItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NonPerishables",
                table: "InwardItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "NonPerishablesWeight",
                table: "InwardItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Perishables",
                table: "InwardItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PerishablesWeight",
                table: "InwardItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Frozen",
                table: "Hampers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "FrozenWeight",
                table: "Hampers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NonFood",
                table: "Hampers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "NonFoodWeight",
                table: "Hampers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NonPerishables",
                table: "Hampers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "NonPerishablesWeight",
                table: "Hampers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Perishables",
                table: "Hampers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PerishablesWeight",
                table: "Hampers",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApprovalNotifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalNotifications", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "Approvals",
                columns: table => new
                {
                    ApprovalQueueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModuleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<int>(type: "int", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Approvals", x => x.ApprovalQueueId);
                });

            migrationBuilder.CreateTable(
                name: "PhotoSpeaks",
                columns: table => new
                {
                    PhotoSpeakId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoSpeaks", x => x.PhotoSpeakId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalNotifications");

            migrationBuilder.DropTable(
                name: "Approvals");

            migrationBuilder.DropTable(
                name: "PhotoSpeaks");

            migrationBuilder.DropColumn(
                name: "Frozen",
                table: "Wastes");

            migrationBuilder.DropColumn(
                name: "FrozenWeight",
                table: "Wastes");

            migrationBuilder.DropColumn(
                name: "NonFood",
                table: "Wastes");

            migrationBuilder.DropColumn(
                name: "NonFoodWeight",
                table: "Wastes");

            migrationBuilder.DropColumn(
                name: "NonPerishables",
                table: "Wastes");

            migrationBuilder.DropColumn(
                name: "NonPerishablesWeight",
                table: "Wastes");

            migrationBuilder.DropColumn(
                name: "Perishables",
                table: "Wastes");

            migrationBuilder.DropColumn(
                name: "PerishablesWeight",
                table: "Wastes");

            migrationBuilder.DropColumn(
                name: "Frozen",
                table: "VolunteerGiveOuts");

            migrationBuilder.DropColumn(
                name: "FrozenWeight",
                table: "VolunteerGiveOuts");

            migrationBuilder.DropColumn(
                name: "NonFood",
                table: "VolunteerGiveOuts");

            migrationBuilder.DropColumn(
                name: "NonFoodWeight",
                table: "VolunteerGiveOuts");

            migrationBuilder.DropColumn(
                name: "NonPerishables",
                table: "VolunteerGiveOuts");

            migrationBuilder.DropColumn(
                name: "NonPerishablesWeight",
                table: "VolunteerGiveOuts");

            migrationBuilder.DropColumn(
                name: "Perishables",
                table: "VolunteerGiveOuts");

            migrationBuilder.DropColumn(
                name: "PerishablesWeight",
                table: "VolunteerGiveOuts");

            migrationBuilder.DropColumn(
                name: "Adults",
                table: "PartnerGiveOuts");

            migrationBuilder.DropColumn(
                name: "Children",
                table: "PartnerGiveOuts");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "PartnerGiveOuts");

            migrationBuilder.DropColumn(
                name: "Frozen",
                table: "PartnerGiveOuts");

            migrationBuilder.DropColumn(
                name: "FrozenWeight",
                table: "PartnerGiveOuts");

            migrationBuilder.DropColumn(
                name: "NonFood",
                table: "PartnerGiveOuts");

            migrationBuilder.DropColumn(
                name: "NonFoodWeight",
                table: "PartnerGiveOuts");

            migrationBuilder.DropColumn(
                name: "NonPerishables",
                table: "PartnerGiveOuts");

            migrationBuilder.DropColumn(
                name: "NonPerishablesWeight",
                table: "PartnerGiveOuts");

            migrationBuilder.DropColumn(
                name: "NumberOfFamilies",
                table: "PartnerGiveOuts");

            migrationBuilder.DropColumn(
                name: "Perishables",
                table: "PartnerGiveOuts");

            migrationBuilder.DropColumn(
                name: "PerishablesWeight",
                table: "PartnerGiveOuts");

            migrationBuilder.DropColumn(
                name: "Seniors",
                table: "PartnerGiveOuts");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "InwardItems");

            migrationBuilder.DropColumn(
                name: "Frozen",
                table: "InwardItems");

            migrationBuilder.DropColumn(
                name: "FrozenWeight",
                table: "InwardItems");

            migrationBuilder.DropColumn(
                name: "NonFood",
                table: "InwardItems");

            migrationBuilder.DropColumn(
                name: "NonFoodWeight",
                table: "InwardItems");

            migrationBuilder.DropColumn(
                name: "NonPerishables",
                table: "InwardItems");

            migrationBuilder.DropColumn(
                name: "NonPerishablesWeight",
                table: "InwardItems");

            migrationBuilder.DropColumn(
                name: "Perishables",
                table: "InwardItems");

            migrationBuilder.DropColumn(
                name: "PerishablesWeight",
                table: "InwardItems");

            migrationBuilder.DropColumn(
                name: "Frozen",
                table: "Hampers");

            migrationBuilder.DropColumn(
                name: "FrozenWeight",
                table: "Hampers");

            migrationBuilder.DropColumn(
                name: "NonFood",
                table: "Hampers");

            migrationBuilder.DropColumn(
                name: "NonFoodWeight",
                table: "Hampers");

            migrationBuilder.DropColumn(
                name: "NonPerishables",
                table: "Hampers");

            migrationBuilder.DropColumn(
                name: "NonPerishablesWeight",
                table: "Hampers");

            migrationBuilder.DropColumn(
                name: "Perishables",
                table: "Hampers");

            migrationBuilder.DropColumn(
                name: "PerishablesWeight",
                table: "Hampers");
        }
    }
}
