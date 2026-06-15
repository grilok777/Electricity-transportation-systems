using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSyncArchitecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateRegistration = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OwnerDeals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlaceLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConclusionDeal = table.Column<DateOnly>(type: "date", nullable: false),
                    CompetionDeal = table.Column<DateOnly>(type: "date", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerDeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OwnerDeals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PowerPlants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxCapacityKw = table.Column<float>(type: "real", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCommissioning = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDecommissioning = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerPlants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PowerPlants_OwnerDeals_DealId",
                        column: x => x.DealId,
                        principalTable: "OwnerDeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PowerPlantDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    ForecastDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerPlantDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PowerPlantDays_PowerPlants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "PowerPlants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HourDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PowerPlantDayId = table.Column<int>(type: "int", nullable: false),
                    ForecastTimestamp = table.Column<TimeOnly>(type: "time", nullable: false),
                    ForecastedKw = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HourDatas_PowerPlantDays_PowerPlantDayId",
                        column: x => x.PowerPlantDayId,
                        principalTable: "PowerPlantDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HourDatas_PowerPlantDayId",
                table: "HourDatas",
                column: "PowerPlantDayId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDeals_UserId",
                table: "OwnerDeals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PowerPlantDays_LastModifiedAt",
                table: "PowerPlantDays",
                column: "LastModifiedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PowerPlantDays_PlantId",
                table: "PowerPlantDays",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_PowerPlants_DealId",
                table: "PowerPlants",
                column: "DealId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HourDatas");

            migrationBuilder.DropTable(
                name: "PowerPlantDays");

            migrationBuilder.DropTable(
                name: "PowerPlants");

            migrationBuilder.DropTable(
                name: "OwnerDeals");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
