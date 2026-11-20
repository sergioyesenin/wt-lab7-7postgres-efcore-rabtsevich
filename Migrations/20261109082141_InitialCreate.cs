using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace wt_lab7_7postgres_efcore_rabtsevich.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aircrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Manufacturer = table.Column<string>(type: "text", nullable: false),
                    SeatCapacity = table.Column<int>(type: "integer", nullable: false),
                    ManufactureYear = table.Column<int>(type: "integer", nullable: false),
                    AirlineId = table.Column<int>(type: "integer", nullable: false),
                    TailNumber = table.Column<string>(type: "text", nullable: false),
                    IsOperational = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircrafts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IataCode = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    FoundedYear = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Rating = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FlightNumber = table.Column<string>(type: "text", nullable: false),
                    Departure = table.Column<string>(type: "text", nullable: false),
                    Destination = table.Column<string>(type: "text", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TicketPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    SeatsAvailable = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    AirlineId = table.Column<int>(type: "integer", nullable: false),
                    AircraftId = table.Column<int>(type: "integer", nullable: false),
                    FlightType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Aircrafts",
                columns: new[] { "Id", "AirlineId", "CreatedAt", "IsOperational", "ManufactureYear", "Manufacturer", "Model", "SeatCapacity", "TailNumber" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2018, "Airbus", "Airbus A320", 180, "RA-89001" },
                    { 2, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2016, "Boeing", "Boeing 737-800", 189, "VQ-BRP" },
                    { 3, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2014, "Airbus", "Airbus A380", 517, "A6-EVK" }
                });

            migrationBuilder.InsertData(
                table: "Airlines",
                columns: new[] { "Id", "Country", "CreatedAt", "Description", "FoundedYear", "IataCode", "IsActive", "Name", "Rating" },
                values: new object[,]
                {
                    { 1, "Россия", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Крупнейшая авиакомпания России.", 1923, "SU", true, "Аэрофлот", 4.7m },
                    { 2, "Россия", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1957, "S7", true, "S7 Airlines", 4.5m },
                    { 3, "ОАЭ", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1985, "EK", true, "Emirates", 4.9m }
                });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "Id", "AircraftId", "AirlineId", "CreatedAt", "Departure", "DepartureTime", "Destination", "FlightNumber", "FlightType", "IsActive", "SeatsAvailable", "TicketPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Москва", new DateTime(2025, 5, 10, 8, 30, 0, 0, DateTimeKind.Utc), "Санкт-Петербург", "SU-100", "Пассажирский", true, 120, 4500m },
                    { 2, 1, 1, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Москва", new DateTime(2025, 5, 11, 14, 0, 0, 0, DateTimeKind.Utc), "Сочи", "SU-205", "Пассажирский", true, 45, 7800m },
                    { 3, 2, 2, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Казань", new DateTime(2025, 5, 12, 6, 15, 0, 0, DateTimeKind.Utc), "Новосибирск", "S7-310", "Пассажирский", false, 0, 6200m },
                    { 4, 3, 3, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Дубай", new DateTime(2025, 5, 13, 18, 45, 0, 0, DateTimeKind.Utc), "Москва", "EK-412", "Пассажирский", true, 78, 53000m },
                    { 5, 1, 1, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Москва", new DateTime(2025, 5, 15, 11, 0, 0, 0, DateTimeKind.Utc), "Владивосток", "SU-777", "Пассажирский", true, 95, 18500m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Airlines_IataCode",
                table: "Airlines",
                column: "IataCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FlightNumber",
                table: "Flights",
                column: "FlightNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aircrafts");

            migrationBuilder.DropTable(
                name: "Airlines");

            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
