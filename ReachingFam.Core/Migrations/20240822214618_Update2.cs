using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReachingFam.Core.Migrations
{
    /// <inheritdoc />
    public partial class Update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Option",
                table: "FoodItemOptions");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReorderLevel",
                table: "FoodItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "OptionTypeId",
                table: "FoodItemOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OptionValueId",
                table: "FoodItemOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FoodItemSubstitutes",
                columns: table => new
                {
                    FoodItemSubstituteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubstituteFoodItemId = table.Column<int>(type: "int", nullable: false),
                    FoodItemId = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItemSubstitutes", x => x.FoodItemSubstituteId);
                    table.ForeignKey(
                        name: "FK_FoodItemSubstitutes_FoodItems_FoodItemId",
                        column: x => x.FoodItemId,
                        principalTable: "FoodItems",
                        principalColumn: "FoodItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FoodItemSubstitutes_FoodItems_SubstituteFoodItemId",
                        column: x => x.SubstituteFoodItemId,
                        principalTable: "FoodItems",
                        principalColumn: "FoodItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OptionTypes",
                columns: table => new
                {
                    OptionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionTypes", x => x.OptionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "OptionValues",
                columns: table => new
                {
                    OptionValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionValues", x => x.OptionValueId);
                    table.ForeignKey(
                        name: "FK_OptionValues_OptionTypes_OptionTypeId",
                        column: x => x.OptionTypeId,
                        principalTable: "OptionTypes",
                        principalColumn: "OptionTypeId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodItemOptions_OptionTypeId",
                table: "FoodItemOptions",
                column: "OptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItemOptions_OptionValueId",
                table: "FoodItemOptions",
                column: "OptionValueId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItemSubstitutes_FoodItemId",
                table: "FoodItemSubstitutes",
                column: "FoodItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItemSubstitutes_SubstituteFoodItemId",
                table: "FoodItemSubstitutes",
                column: "SubstituteFoodItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionValues_OptionTypeId",
                table: "OptionValues",
                column: "OptionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItemOptions_OptionTypes_OptionTypeId",
                table: "FoodItemOptions",
                column: "OptionTypeId",
                principalTable: "OptionTypes",
                principalColumn: "OptionTypeId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItemOptions_OptionValues_OptionValueId",
                table: "FoodItemOptions",
                column: "OptionValueId",
                principalTable: "OptionValues",
                principalColumn: "OptionValueId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItemOptions_OptionTypes_OptionTypeId",
                table: "FoodItemOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItemOptions_OptionValues_OptionValueId",
                table: "FoodItemOptions");

            migrationBuilder.DropTable(
                name: "FoodItemSubstitutes");

            migrationBuilder.DropTable(
                name: "OptionValues");

            migrationBuilder.DropTable(
                name: "OptionTypes");

            migrationBuilder.DropIndex(
                name: "IX_FoodItemOptions_OptionTypeId",
                table: "FoodItemOptions");

            migrationBuilder.DropIndex(
                name: "IX_FoodItemOptions_OptionValueId",
                table: "FoodItemOptions");

            migrationBuilder.DropColumn(
                name: "OptionTypeId",
                table: "FoodItemOptions");

            migrationBuilder.DropColumn(
                name: "OptionValueId",
                table: "FoodItemOptions");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReorderLevel",
                table: "FoodItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option",
                table: "FoodItemOptions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
