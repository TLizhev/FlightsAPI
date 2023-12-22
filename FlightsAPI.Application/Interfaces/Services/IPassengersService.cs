using FlightsAPI.Domain.Models;

namespace FlightsAPI.Application.Interfaces.Services
{
    public interface IPassengersService
    {
        List<Passenger> GetPassengers();
        Passenger GetPassenger(int id);
        Task AddPassenger(Passenger passenger);
        void EditPassenger(Passenger passenger);
        void DeletePassenger(int id);
    }
}
