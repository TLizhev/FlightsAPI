namespace FlightsAPI.Domain.Models
{
    public class Plane
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Seats { get; set; }
        public int Range { get; set; }
    }
}
