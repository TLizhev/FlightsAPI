using FlightsAPI.Data;
using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
{
    public class PlaneService : IPlaneService
    {
        private readonly ApplicationDbContext _db;

        public PlaneService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Plane> GetPlanes()
        {
            return _db.Planes.ToList();
        }

        public Plane GetPlane(int id)
        {
            return _db.Planes.FirstOrDefault(x => x.Id == id);
        }
    }
}
