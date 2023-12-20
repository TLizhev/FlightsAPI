using FlightsAPI.Domain.Models;

namespace FlightsAPI.Services
{
    public interface IPlanesService
    {
        List<Plane> GetPlanes();
        Plane GetPlane(int id);
        Plane GetMostSeats();
        Plane GetBiggestRange();
        Task AddPlane(Plane plane);
        void EditPlane(Plane plane);
        void DeletePlane(int id);
    }
}
