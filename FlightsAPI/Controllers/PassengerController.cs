using FlightsAPI.Data.Models;
using FlightsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers
{
    [ApiController]
    [Route("/api/passengers")]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerService _passengerService;

        public PassengerController(IPassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        [HttpGet]
        public List<Passenger> GetPassengerList()
        {
            return _passengerService.GetPassengers();
        }

        [HttpGet]
        [Route("{id:int}")]
        public Passenger GetPassenger(int id)
        {
            return _passengerService.GetPassenger(id);
        }
    }
}
