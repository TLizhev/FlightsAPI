using FlightsAPI.Data;
using FlightsAPI.Data.Models;

namespace FlightsAPI.Repositories
{
    public class PassengersRepository : IPassengersRepository
    {
        private readonly ApplicationDbContext _db;

        public PassengersRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Passenger GetById(int id)
        {
            return _db.Passengers.FirstOrDefault(x => x.Id == id)!;
        }

        public List<Passenger> GetAll()
        {
            return _db.Passengers.ToList();
        }

        public async Task AddAsync(Passenger entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public void Update(Passenger entity)
        {
            _db.Update(entity);
            _db.SaveChanges();
        }

        public void Delete(Passenger entity)
        {
            _db.Remove(entity);
            _db.SaveChanges();
        }
    }
}
