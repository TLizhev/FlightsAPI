using FlightsAPI.Data;
using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
{
    public class FlightsService : IFlightsService
    {
        private readonly ApplicationDbContext _db;

        public FlightsService(ApplicationDbContext db)
        {
            this._db = db;
        }

        public Flight GetFlight(int id)
        {
            return _db.Flights.FirstOrDefault(x => x.Id == id)!;
        }

        public List<Flight> GetFlights()
        {
            return _db.Flights.ToList();
        }
    }
}