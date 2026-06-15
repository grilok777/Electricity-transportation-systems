using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialGridArchitecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Datetimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvailablePower = table.Column<float>(type: "real", nullable: false),
                    RequiredPower = table.Column<float>(type: "real", nullable: false),
                    ForecastDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ForecastTimestamp = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datetimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccessLevel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.Id);
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
                    CompetionDeal = table.Column<DateOnly>(type: "date", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerDeals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Substations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxThroughputMw = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Substations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubstationLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubstationId = table.Column<int>(type: "int", nullable: false),
                    LineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseLoadKw = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubstationLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubstationLines_Substations_SubstationId",
                        column: x => x.SubstationId,
                        principalTable: "Substations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AvailablePowers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubstationLineId = table.Column<int>(type: "int", nullable: false),
                    DatetimeId = table.Column<int>(type: "int", nullable: false),
                    AvailablePowerPlants = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailablePowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvailablePowers_Datetimes_DatetimeId",
                        column: x => x.DatetimeId,
                        principalTable: "Datetimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvailablePowers_SubstationLines_SubstationLineId",
                        column: x => x.SubstationLineId,
                        principalTable: "SubstationLines",
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
                    SubstationLineId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxCapacityKw = table.Column<float>(type: "real", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCommissioning = table.Column<DateOnly>(type: "date", nullable: false),
                    DateDecommissioning = table.Column<DateOnly>(type: "date", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_PowerPlants_SubstationLines_SubstationLineId",
                        column: x => x.SubstationLineId,
                        principalTable: "SubstationLines",
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
                name: "IX_AvailablePowers_DatetimeId",
                table: "AvailablePowers",
                column: "DatetimeId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailablePowers_SubstationLineId",
                table: "AvailablePowers",
                column: "SubstationLineId");

            migrationBuilder.CreateIndex(
                name: "IX_HourDatas_PowerPlantDayId",
                table: "HourDatas",
                column: "PowerPlantDayId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerDeals_LastModifiedAt",
                table: "OwnerDeals",
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
                name: "IX_PowerPlants_LastModifiedAt",
                table: "PowerPlants",
                column: "LastModifiedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PowerPlants_SubstationLineId",
                table: "PowerPlants",
                column: "SubstationLineId");

            migrationBuilder.CreateIndex(
                name: "IX_SubstationLines_SubstationId",
                table: "SubstationLines",
                column: "SubstationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailablePowers");

            migrationBuilder.DropTable(
                name: "HourDatas");

            migrationBuilder.DropTable(
                name: "Operators");

            migrationBuilder.DropTable(
                name: "Datetimes");

            migrationBuilder.DropTable(
                name: "PowerPlantDays");

            migrationBuilder.DropTable(
                name: "PowerPlants");

            migrationBuilder.DropTable(
                name: "OwnerDeals");

            migrationBuilder.DropTable(
                name: "SubstationLines");

            migrationBuilder.DropTable(
                name: "Substations");
        }
    }
}
