namespace FlightsAPI.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int PassengerId { get; set; }
        public int FlightId { get; set; }
        public int LuggageId { get; set; }
        public decimal Price { get; set; }
    }
}
