using System.Linq.Expressions;
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

        public List<Flight> GetTopFiveFlights()
        {
            var stuff = _db.Flights.Select(x => x.Destination).ToList();
            var stuff1 = _db.Flights.Select(x => x.Origin).ToList();
            var dist = stuff.Distinct().ToList();
            var dist1 = stuff1.Distinct().ToList();
            var filter =
                stuff.GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Select(y => y)
                        .Count());
            return new();
        }

        public List<Flight> GetFlights()
        {
            return _db.Flights.ToList();
        }
    }
}