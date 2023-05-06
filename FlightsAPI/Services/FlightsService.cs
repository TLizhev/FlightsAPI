using FlightsAPI.Data;
using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
{
    public class FlightsService : IFlightsService
    {
        private readonly ApplicationDbContext db;

        public FlightsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public Flight GetFlight(int id)
        {
            return db.Flights.FirstOrDefault(x => x.Id == id);
        }

        public List<Flight> GetFlights()
        {
            return db.Flights.ToList();
        }
    }
}