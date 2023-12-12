using FlightsAPI.Data;
using FlightsAPI.Data.Models;
using FlightsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers
{
    [ApiController]
    [Route(Endpoints.BasePassengersEndpoint)]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerService _passengerService;

        public PassengerController(IPassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Passenger), 200)]
        [ProducesResponseType(204)]
        public IActionResult GetPassengerList()
        {
            var passengers = _passengerService.GetPassengers();

            if (passengers is { Count: 0 })
                return NoContent();

            return Ok(passengers);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(Passenger), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetPassenger(int id)
        {
            try
            {
                var passenger = _passengerService.GetPassenger(id);
                return Ok(passenger);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Flight), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddPassenger(
            string firstName,
            string lastName,
            int age,
            string address,
            string passportId)
        {
            try
            {
                var passenger = new Passenger
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age,
                    Address = address,
                    PassportId = passportId
                };

                await _passengerService.AddPassenger(passenger);
                return CreatedAtRoute("GetFlight", new { flightId = passenger.Id }, passenger);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(Passenger), 200)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePassenger(
            int id, 
            string firstName, 
            string lastName, 
            int age, 
            string address, 
            string passportId)
        {
            var newPassenger = new Passenger
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Address = address,
                PassportId = passportId
            };

            try
            {
                _passengerService.EditPassenger(newPassenger);
                return Ok(newPassenger);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(Flight), 204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePassenger(int id)
        {
            try
            {
                _passengerService.DeletePassenger(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
