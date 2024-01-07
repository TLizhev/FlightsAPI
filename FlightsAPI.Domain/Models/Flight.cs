namespace FlightsAPI.Domain.Models;

public class Flight
{
    public int Id { get; set; }
    public DateTime? DepartureTime { get; set; }
    public DateTime? ArrivalTime { get; set; }
    public string Origin { get; set; } = null!;
    public string Destination { get; set; } = null!;
    public int PlaneId { get; set; }
}