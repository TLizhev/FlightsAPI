using FlightsAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Services
{
    public interface IFlightsService
    {
        List<Flight> GetFlights();
        Flight GetFlight(int id);
        List<TopFiveDto> GetTopFiveFlightOrigins();
        List<TopFiveDto> GetTopFiveFlightDestinations();
        List<TopFiveDto> TopFiveFlights(string direction);
        Task<IActionResult> AddFlight(DateTime? arrivalTime,
            DateTime? departureTime,
            string origin,
            string destination,
            int planeId);

        IActionResult EditFlight(int id, DateTime? arrivalTime,
            DateTime? departureTime,
            string origin,
            string destination,
            int planeId);

        IActionResult DeleteFlight(int id);
    }
}
