using FlightsAPI.Data.Models;
using FlightsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers
{
    [ApiController]
    [Route("/api/flights")]
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
        [Route("/api/flights/{id}")]
        public Flight GetFlight(int id)
        {
           return _flightService.GetFlight(id);
        }
    }
}
