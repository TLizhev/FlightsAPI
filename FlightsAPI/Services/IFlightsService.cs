using FlightsAPI.Domain.Models;

namespace FlightsAPI.Services
{
    public interface IFlightsService
    {
        List<Flight> GetFlights();
        Flight GetFlight(int id);
        List<TopFiveDto> GetTopFiveFlightOrigins();
        List<TopFiveDto> GetTopFiveFlightDestinations();
        List<TopFiveDto> TopFiveFlights(string direction);
        Task AddFlight(Flight flight);

        void EditFlight(Flight flight);

        void DeleteFlight(int id);
    }
}
