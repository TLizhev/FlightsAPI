using FlightsAPI.Data;
using FlightsAPI.Data.Models;
using FlightsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers
{
    [ApiController]
    [Route(Endpoints.BaseFlightsEndpoint)]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightsService _flightService;

        public FlightsController(IFlightsService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public List<Flight> GetFlights()
        {
           return _flightService.GetFlights();
        } 
        
        [HttpGet]
        [Route("{id:int}")]
        public Flight GetFlight(int id)
        {
           return _flightService.GetFlight(id);
        }

        [HttpGet]
        [Route(Endpoints.TopFlights)]
        public List<TopFiveDto> GetTop5Flights(string direction)
        {
            return _flightService.GetTopFiveFlights(direction);
        }

        [HttpPost]
        [Route(Endpoints.AddFlight)]
        public async Task<IActionResult> AddFlight(string origin,
            string destination,
            DateTime? departureTime,
            DateTime? arrivalTime,
            int planeId)
        {
           return await _flightService.AddFlight(arrivalTime, departureTime, origin, destination, planeId);
        }

        [HttpPatch]
        [Route(Endpoints.PatchFlight)]
        public async Task PatchFlight(int id, 
            string origin,
            string destination,
            DateTime? departureTime,
            DateTime? arrivalTime,
            int planeId)
        {
            await _flightService.EditFlight(id,arrivalTime, departureTime, origin, destination, planeId);
        }

        [HttpDelete]
        [Route(Endpoints.DeleteFlight)]
        public async Task<IActionResult> DeleteFlight(int id)
        {
           return await _flightService.DeleteFlight(id);
        }
    }
}
