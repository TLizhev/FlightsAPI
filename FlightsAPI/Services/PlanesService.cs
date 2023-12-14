using FlightsAPI.Data;
using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
{
    public class PlanesService : IPlanesService
    {
        private readonly ApplicationDbContext _db;

        public PlanesService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Plane> GetPlanes()
        {
            return _db.Planes.ToList();
        }

        public Plane GetPlane(int id)
        {
            return _db.Planes.FirstOrDefault(x => x.Id == id)!;
        }

        public Plane GetMostSeats()
        {
            return _db.Planes.ToList().MaxBy(x => x.Seats)!;
        }

        public Plane GetBiggestRange()
        {
            return _db.Planes.ToList().MaxBy(x => x.Range)!;
        }
    }
}
