namespace FlightsAPI.Domain.Models
{
    public class CabinCrew
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Profession Profession { get; set; }
        public int FlightId { get; set; }
    }

    public enum Profession
    {
        Captain,
        FlightAttendant
    }
}
