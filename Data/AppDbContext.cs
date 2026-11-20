using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_rabtsevich.Models;

namespace wt_lab7_7postgres_efcore_rabtsevich.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    private static DateTime Utc(int y, int m, int d) =>
        new DateTime(y, m, d, 0, 0, 0, DateTimeKind.Utc);

    private static DateTime Utc(int y, int m, int d, int h, int min, int s) =>
        new DateTime(y, m, d, h, min, s, DateTimeKind.Utc);
    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<Airline> Airlines => Set<Airline>();
    public DbSet<Aircraft> Aircrafts => Set<Aircraft>();

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Flight>()
        .HasIndex(f => f.FlightNumber)
        .IsUnique();

    modelBuilder.Entity<Airline>()
        .HasIndex(a => a.IataCode)
        .IsUnique();

    modelBuilder.Entity<Airline>().HasData(
        new Airline { Id = 1, Name = "Аэрофлот", IataCode = "SU", Country = "Россия",
            FoundedYear = 1923, IsActive = true, Rating = 4.7m,
            Description = "Крупнейшая авиакомпания России.",
            CreatedAt = Utc(2025, 1, 1) },
        new Airline { Id = 2, Name = "S7 Airlines", IataCode = "S7", Country = "Россия",
            FoundedYear = 1957, IsActive = true, Rating = 4.5m,
            CreatedAt = Utc(2025, 1, 1) },
        new Airline { Id = 3, Name = "Emirates", IataCode = "EK", Country = "ОАЭ",
            FoundedYear = 1985, IsActive = true, Rating = 4.9m,
            CreatedAt = Utc(2025, 1, 1) }
    );

    modelBuilder.Entity<Aircraft>().HasData(
        new Aircraft { Id = 1, Model = "Airbus A320", Manufacturer = "Airbus",
            SeatCapacity = 180, ManufactureYear = 2018, AirlineId = 1,
            TailNumber = "RA-89001", IsOperational = true,
            CreatedAt = Utc(2025, 1, 1) },
        new Aircraft { Id = 2, Model = "Boeing 737-800", Manufacturer = "Boeing",
            SeatCapacity = 189, ManufactureYear = 2016, AirlineId = 2,
            TailNumber = "VQ-BRP", IsOperational = true,
            CreatedAt = Utc(2025, 1, 1) },
        new Aircraft { Id = 3, Model = "Airbus A380", Manufacturer = "Airbus",
            SeatCapacity = 517, ManufactureYear = 2014, AirlineId = 3,
            TailNumber = "A6-EVK", IsOperational = true,
            CreatedAt = Utc(2025, 1, 1) }
    );

    modelBuilder.Entity<Flight>().HasData(
        new Flight { Id = 1, FlightNumber = "SU-100", Departure = "Москва",
            Destination = "Санкт-Петербург", DepartureTime = Utc(2025, 5, 10, 8, 30, 0),
            TicketPrice = 4500, SeatsAvailable = 120, IsActive = true,
            AirlineId = 1, AircraftId = 1, FlightType = "Пассажирский",
            CreatedAt = Utc(2025, 1, 2) },
        new Flight { Id = 2, FlightNumber = "SU-205", Departure = "Москва",
            Destination = "Сочи", DepartureTime = Utc(2025, 5, 11, 14, 0, 0),
            TicketPrice = 7800, SeatsAvailable = 45, IsActive = true,
            AirlineId = 1, AircraftId = 1, FlightType = "Пассажирский",
            CreatedAt = Utc(2025, 1, 2) },
        new Flight { Id = 3, FlightNumber = "S7-310", Departure = "Казань",
            Destination = "Новосибирск", DepartureTime = Utc(2025, 5, 12, 6, 15, 0),
            TicketPrice = 6200, SeatsAvailable = 0, IsActive = false,
            AirlineId = 2, AircraftId = 2, FlightType = "Пассажирский",
            CreatedAt = Utc(2025, 1, 2) },
        new Flight { Id = 4, FlightNumber = "EK-412", Departure = "Дубай",
            Destination = "Москва", DepartureTime = Utc(2025, 5, 13, 18, 45, 0),
            TicketPrice = 53000, SeatsAvailable = 78, IsActive = true,
            AirlineId = 3, AircraftId = 3, FlightType = "Пассажирский",
            CreatedAt = Utc(2025, 1, 2) },
        new Flight { Id = 5, FlightNumber = "SU-777", Departure = "Москва",
            Destination = "Владивосток", DepartureTime = Utc(2025, 5, 15, 11, 0, 0),
            TicketPrice = 18500, SeatsAvailable = 95, IsActive = true,
            AirlineId = 1, AircraftId = 1, FlightType = "Пассажирский",
            CreatedAt = Utc(2025, 1, 2) }
        );
    }
}