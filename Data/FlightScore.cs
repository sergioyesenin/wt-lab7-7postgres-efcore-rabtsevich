using wt_lab6_6minimalapi_rabtsevich.Models;

namespace wt_lab6_6minimalapi_rabtsevich.Data;

public static class FlightStore
{
    public static List<Airline> Airlines { get; } = new()
    {
        new Airline { Id = 1, Name = "Аэрофлот", IataCode = "SU", Country = "Россия",
            FoundedYear = 1923, IsActive = true, Rating = 4.7m,
            Description = "Крупнейшая авиакомпания России." },
        new Airline { Id = 2, Name = "S7 Airlines", IataCode = "S7", Country = "Россия",
            FoundedYear = 1957, IsActive = true, Rating = 4.5m },
        new Airline { Id = 3, Name = "Emirates", IataCode = "EK", Country = "ОАЭ",
            FoundedYear = 1985, IsActive = true, Rating = 4.9m },
    };

    public static List<Aircraft> Aircrafts { get; } = new()
    {
        new Aircraft { Id = 1, Model = "Airbus A320", Manufacturer = "Airbus",
            SeatCapacity = 180, ManufactureYear = 2018, AirlineId = 1,
            TailNumber = "RA-89001", IsOperational = true },
        new Aircraft { Id = 2, Model = "Boeing 737-800", Manufacturer = "Boeing",
            SeatCapacity = 189, ManufactureYear = 2016, AirlineId = 2,
            TailNumber = "VQ-BRP", IsOperational = true },
        new Aircraft { Id = 3, Model = "Airbus A380", Manufacturer = "Airbus",
            SeatCapacity = 517, ManufactureYear = 2014, AirlineId = 3,
            TailNumber = "A6-EVK", IsOperational = true },
    };

    public static List<Flight> Flights { get; } = new()
    {
        new Flight { Id = 1, FlightNumber = "SU-100", Departure = "Москва",
            Destination = "Санкт-Петербург", DepartureTime = new DateTime(2025, 5, 10, 8, 30, 0),
            TicketPrice = 4500, SeatsAvailable = 120, IsActive = true,
            AirlineId = 1, AircraftId = 1, FlightType = "Пассажирский" },
        new Flight { Id = 2, FlightNumber = "SU-205", Departure = "Москва",
            Destination = "Сочи", DepartureTime = new DateTime(2025, 5, 11, 14, 0, 0),
            TicketPrice = 7800, SeatsAvailable = 45, IsActive = true,
            AirlineId = 1, AircraftId = 1, FlightType = "Пассажирский" },
        new Flight { Id = 3, FlightNumber = "S7-310", Departure = "Казань",
            Destination = "Новосибирск", DepartureTime = new DateTime(2025, 5, 12, 6, 15, 0),
            TicketPrice = 6200, SeatsAvailable = 0, IsActive = false,
            AirlineId = 2, AircraftId = 2, FlightType = "Пассажирский" },
        new Flight { Id = 4, FlightNumber = "EK-412", Departure = "Дубай",
            Destination = "Москва", DepartureTime = new DateTime(2025, 5, 13, 18, 45, 0),
            TicketPrice = 53000, SeatsAvailable = 78, IsActive = true,
            AirlineId = 3, AircraftId = 3, FlightType = "Пассажирский" },
        new Flight { Id = 5, FlightNumber = "SU-777", Departure = "Москва",
            Destination = "Владивосток", DepartureTime = new DateTime(2025, 5, 15, 11, 0, 0),
            TicketPrice = 18500, SeatsAvailable = 95, IsActive = true,
            AirlineId = 1, AircraftId = 1, FlightType = "Пассажирский" },
    };

    // Вспомогательные методы
    public static int NextFlightId() =>
        Flights.Any() ? Flights.Max(f => f.Id) + 1 : 1;

    public static Flight? GetFlight(int id) =>
        Flights.FirstOrDefault(f => f.Id == id);

    public static bool FlightNumberExists(string number, int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(number)) return false;
        var n = number.Trim().ToUpperInvariant();
        return Flights.Any(f => f.FlightNumber.ToUpperInvariant() == n && f.Id != excludeId);
    }
}