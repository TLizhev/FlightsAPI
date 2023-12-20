namespace FlightsAPI.Domain.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Age { get; set; }
        public string Address { get; set; } = null!;
        public string PassportId { get; set; } = null!;
    }
}
