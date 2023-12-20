using System.Diagnostics.CodeAnalysis;
using FlightsAPI.Domain.Models;
using FlightsAPI.Infra;

namespace FlightsAPI.Repositories
{
    [ExcludeFromCodeCoverage]
    public class FlightsRepository : IFlightsRepository
    {
        private readonly ApplicationDbContext _db;

        public FlightsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Flight GetById(int id)
        {
            return _db.Flights.SingleOrDefault(x => x.Id == id)!;
        }

        public List<Flight> GetAll()
        {
            return _db.Flights.ToList();
        }

        public async Task AddAsync(Flight flight)
        {
            await _db.Flights.AddAsync(flight);
            await _db.SaveChangesAsync();
        }

        public void Update(Flight flight)
        {
            _db.Flights.Update(flight);
            _db.SaveChanges();
        }

        public void Delete(Flight flight)
        {
            _db.Flights.Remove(flight);
            _db.SaveChanges();
        }
    }
}
