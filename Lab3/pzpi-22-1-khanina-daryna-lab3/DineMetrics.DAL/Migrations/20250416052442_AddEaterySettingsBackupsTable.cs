using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DineMetrics.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddEaterySettingsBackupsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EaterySettingsBackups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EateryId = table.Column<int>(type: "int", nullable: false),
                    BackupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackupTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperatingHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaximumCapacity = table.Column<int>(type: "int", nullable: false),
                    TemperatureThreshold = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EaterySettingsBackups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EaterySettingsBackups_Eateries_EateryId",
                        column: x => x.EateryId,
                        principalTable: "Eateries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "83fcd3f7129b081faeb043dc07262e63fea599da4be6869a7a1780f7084a15b4");

            migrationBuilder.CreateIndex(
                name: "IX_EaterySettingsBackups_EateryId",
                table: "EaterySettingsBackups",
                column: "EateryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EaterySettingsBackups");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "f9c355b602a10ee3e31c2f2c23acdcba3b299ddcf9607ba0d10ae9d041e8e09b");
        }
    }
}
