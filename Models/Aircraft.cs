namespace wt_lab7_7postgres_efcore_rabtsevich.Models;

public class Aircraft
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Model { get; set; } = "";
    public string Manufacturer { get; set; } = "";
    public int SeatCapacity { get; set; }
    public int ManufactureYear { get; set; }
    public int AirlineId { get; set; }
    public string TailNumber { get; set; } = "";
    public bool IsOperational { get; set; } = true;
}