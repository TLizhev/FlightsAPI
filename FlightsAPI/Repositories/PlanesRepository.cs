using FlightsAPI.Data;
using FlightsAPI.Data.Models;

namespace FlightsAPI.Repositories
{
    public class PlanesRepository : IPlanesRepository
    {
        private readonly ApplicationDbContext _db;

        public PlanesRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Plane GetById(int id)
        {
            return _db.Planes.FirstOrDefault(x => x.Id == id)!;
        }

        public List<Plane> GetAll()
        {
            return _db.Planes.ToList();
        }

        public async Task AddAsync(Plane entity)
        {
            await _db.Planes.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public void Update(Plane entity)
        {
            _db.Update(entity);
            _db.SaveChanges();
        }

        public void Delete(Plane entity)
        {
            _db.Remove(entity);
            _db.SaveChanges();
        }
    }
}
