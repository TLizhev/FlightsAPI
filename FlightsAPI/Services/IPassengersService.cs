using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
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
