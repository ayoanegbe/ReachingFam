using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReachingFam.Core.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_ItemCategory_ItemCategoryId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_UnitOfMeasures_UnitOfMeasureId",
                table: "Stocks");

            migrationBuilder.DropTable(
                name: "StockHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemCategory",
                table: "ItemCategory");

            migrationBuilder.DropColumn(
                name: "ReservedQuantity",
                table: "Stocks");

            migrationBuilder.RenameTable(
                name: "ItemCategory",
                newName: "ItemCategories");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasureId",
                table: "Stocks",
                newName: "DonorId");

            migrationBuilder.RenameIndex(
                name: "IX_Stocks_UnitOfMeasureId",
                table: "Stocks",
                newName: "IX_Stocks_DonorId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateReceived",
                table: "Stocks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "HamperItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "HamperItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "HamperItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HamperId",
                table: "HamperItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "HamperItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "HamperItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReorderLevel",
                table: "FoodItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "UnitOfMeasureId",
                table: "FoodItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "ItemCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "ItemCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "ItemCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ItemCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemCategories",
                table: "ItemCategories",
                column: "ItemCategoryId");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "PartnerHamperItems",
                columns: table => new
                {
                    PartnerHamperItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerGiveOutId = table.Column<int>(type: "int", nullable: false),
                    FoodItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerHamperItems", x => x.PartnerHamperItemId);
                    table.ForeignKey(
                        name: "FK_PartnerHamperItems_FoodItems_FoodItemId",
                        column: x => x.FoodItemId,
                        principalTable: "FoodItems",
                        principalColumn: "FoodItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartnerHamperItems_PartnerGiveOuts_PartnerGiveOutId",
                        column: x => x.PartnerGiveOutId,
                        principalTable: "PartnerGiveOuts",
                        principalColumn: "PartnerGiveOutId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    StockTransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.StockTransactionId);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerHamperItems",
                columns: table => new
                {
                    VolunteerHamperItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VolunteerGiveOutId = table.Column<int>(type: "int", nullable: false),
                    FoodItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerHamperItems", x => x.VolunteerHamperItemId);
                    table.ForeignKey(
                        name: "FK_VolunteerHamperItems_FoodItems_FoodItemId",
                        column: x => x.FoodItemId,
                        principalTable: "FoodItems",
                        principalColumn: "FoodItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VolunteerHamperItems_VolunteerGiveOuts_VolunteerGiveOutId",
                        column: x => x.VolunteerGiveOutId,
                        principalTable: "VolunteerGiveOuts",
                        principalColumn: "VolunteerGiveOutId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HamperItems_HamperId",
                table: "HamperItems",
                column: "HamperId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_UnitOfMeasureId",
                table: "FoodItems",
                column: "UnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerHamperItems_FoodItemId",
                table: "PartnerHamperItems",
                column: "FoodItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerHamperItems_PartnerGiveOutId",
                table: "PartnerHamperItems",
                column: "PartnerGiveOutId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_StockId",
                table: "StockTransactions",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerHamperItems_FoodItemId",
                table: "VolunteerHamperItems",
                column: "FoodItemId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerHamperItems_VolunteerGiveOutId",
                table: "VolunteerHamperItems",
                column: "VolunteerGiveOutId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_ItemCategories_ItemCategoryId",
                table: "FoodItems",
                column: "ItemCategoryId",
                principalTable: "ItemCategories",
                principalColumn: "ItemCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_UnitOfMeasures_UnitOfMeasureId",
                table: "FoodItems",
                column: "UnitOfMeasureId",
                principalTable: "UnitOfMeasures",
                principalColumn: "UnitOfMeasureId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HamperItems_Hampers_HamperId",
                table: "HamperItems",
                column: "HamperId",
                principalTable: "Hampers",
                principalColumn: "HamperId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Donors_DonorId",
                table: "Stocks",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "DonorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_ItemCategories_ItemCategoryId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_UnitOfMeasures_UnitOfMeasureId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_HamperItems_Hampers_HamperId",
                table: "HamperItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Donors_DonorId",
                table: "Stocks");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "PartnerHamperItems");

            migrationBuilder.DropTable(
                name: "StockTransactions");

            migrationBuilder.DropTable(
                name: "VolunteerHamperItems");

            migrationBuilder.DropIndex(
                name: "IX_HamperItems_HamperId",
                table: "HamperItems");

            migrationBuilder.DropIndex(
                name: "IX_FoodItems_UnitOfMeasureId",
                table: "FoodItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemCategories",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "DateReceived",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "HamperItems");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "HamperItems");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "HamperItems");

            migrationBuilder.DropColumn(
                name: "HamperId",
                table: "HamperItems");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "HamperItems");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "HamperItems");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "ReorderLevel",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasureId",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ItemCategories");

            migrationBuilder.RenameTable(
                name: "ItemCategories",
                newName: "ItemCategory");

            migrationBuilder.RenameColumn(
                name: "DonorId",
                table: "Stocks",
                newName: "UnitOfMeasureId");

            migrationBuilder.RenameIndex(
                name: "IX_Stocks_DonorId",
                table: "Stocks",
                newName: "IX_Stocks_UnitOfMeasureId");

            migrationBuilder.AddColumn<decimal>(
                name: "ReservedQuantity",
                table: "Stocks",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemCategory",
                table: "ItemCategory",
                column: "ItemCategoryId");

            migrationBuilder.CreateTable(
                name: "StockHistories",
                columns: table => new
                {
                    StockHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodItemId = table.Column<int>(type: "int", nullable: false),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustedQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHistories", x => x.StockHistoryId);
                    table.ForeignKey(
                        name: "FK_StockHistories_FoodItems_FoodItemId",
                        column: x => x.FoodItemId,
                        principalTable: "FoodItems",
                        principalColumn: "FoodItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockHistories_FoodItemId",
                table: "StockHistories",
                column: "FoodItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_ItemCategory_ItemCategoryId",
                table: "FoodItems",
                column: "ItemCategoryId",
                principalTable: "ItemCategory",
                principalColumn: "ItemCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_UnitOfMeasures_UnitOfMeasureId",
                table: "Stocks",
                column: "UnitOfMeasureId",
                principalTable: "UnitOfMeasures",
                principalColumn: "UnitOfMeasureId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
