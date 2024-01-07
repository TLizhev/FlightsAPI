namespace FlightsAPI.Domain.Models;

public class Luggage
{
    public int Id { get; set; }
    public int LuggageTypeId { get; set; }
    public int PassengerId { get; set; }
}