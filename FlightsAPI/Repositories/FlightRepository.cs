using FlightsAPI.Data;
using FlightsAPI.Data.Models;

namespace FlightsAPI.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly ApplicationDbContext _db;

        public FlightRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Flight GetById(int id)
        {
            return _db.Flights.FirstOrDefault(x => x.Id == id)!;
        }

        public List<Flight> GetAll()
        {
            return _db.Flights.ToList();
        }

        public void Add(Flight flight)
        {
            _db.Flights.Add(flight);
            _db.SaveChanges();
        }

        public void Update(Flight flight)
        {
            _db.Flights.Update(flight);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            _db.Flights.Remove(GetById(id));
        }
    }
}
