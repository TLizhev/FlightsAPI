using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
{
    public interface IPassengerService
    {
        List<Passenger> GetPassengers();
        Passenger GetPassenger(int id);
    }
}
