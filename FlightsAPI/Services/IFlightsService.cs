using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
{
    public interface IFlightsService
    {
        List<Flight> GetFlights();

        Flight GetFlight(int id);
    }
}
