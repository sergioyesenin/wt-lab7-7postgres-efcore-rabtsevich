namespace wt_lab7_7postgres_efcore_rabtsevich.Models;

public class Airline
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; } = "";
    public string IataCode { get; set; } = "";
    public string Country { get; set; } = "";
    public int FoundedYear { get; set; }
    public bool IsActive { get; set; } = true;
    public decimal Rating { get; set; }
    public string? Description { get; set; }
}