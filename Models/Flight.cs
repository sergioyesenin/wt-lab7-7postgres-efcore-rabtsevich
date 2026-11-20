namespace wt_lab7_7postgres_efcore_rabtsevich.Models;

public class Flight
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string FlightNumber { get; set; } = "";
    public string Departure { get; set; } = "";
    public string Destination { get; set; } = "";
    public DateTime DepartureTime { get; set; }
    public decimal TicketPrice { get; set; }
    public int SeatsAvailable { get; set; }
    public bool IsActive { get; set; } = true;
    public int AirlineId { get; set; }
    public int AircraftId { get; set; }
    public string FlightType { get; set; } = "Пассажирский";
    public string? Note { get; set; } 
}