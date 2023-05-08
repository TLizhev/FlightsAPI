using FlightsAPI.Data;
using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly ApplicationDbContext _db;

        public PassengerService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Passenger> GetPassengers()
        {
            return _db.Passengers.ToList();
        }

        public Passenger GetPassenger(int id)
        {
            return _db.Passengers.FirstOrDefault(x => x.Id == id)!;
        }
    }
}
