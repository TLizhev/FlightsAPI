using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
{
    public interface IPlanesService
    {
        List<Plane> GetPlanes();
        Plane GetPlane(int id);
        Plane GetMostSeats();
        Plane GetBiggestRange();
    }
}
