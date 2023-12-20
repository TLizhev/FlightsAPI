using FlightsAPI.Data;
using FlightsAPI.Domain.Models;
using FlightsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers
{
    [ApiController]
    [Route(Endpoints.BaseLuggageEndpoint)]
    public class LuggageController : ControllerBase
    {
        private readonly ILuggageService _luggageService;

        public LuggageController(ILuggageService luggageService)
        {
            _luggageService = luggageService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Luggage>), 200)]
        [ProducesResponseType(204)]
        public IActionResult GetLuggage()
        {
            var luggage = _luggageService.GetLuggage();

            if (luggage is {Count : 0})
                return NoContent();

            return Ok(luggage);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(Luggage), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetLuggage(int id)
        {
            try
            {
                var luggage = _luggageService.GetLuggage(id);
                return Ok(luggage);
            }
            catch (Exception)
            {
                return NotFound("A luggage with this id does not exist.");
            }
        }

        [HttpGet]
        [Route(Endpoints.Popular)]
        [ProducesResponseType(typeof(Luggage), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetPopularLuggage()
        {
            try
            {
                var mostPopular = _luggageService.GetMostPopularLuggage();
                return Ok(mostPopular);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Luggage), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddLuggage(int luggageTypeId, int passengerId)
        {
            try
            {
                var luggage = new Luggage()
                {
                    LuggageTypeId = luggageTypeId,
                    PassengerId = passengerId
                };

                await _luggageService.AddLuggage(luggage);
                return CreatedAtRoute("GetLuggage", new {id = luggage.Id}, luggage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(Flight), 201)]
        [ProducesResponseType(404)]
        public IActionResult EditLuggage(int id, int luggageTypeId, int passengerId)
        {
            var newLuggage = new Luggage()
            {
                Id = id,
                LuggageTypeId = luggageTypeId,
                PassengerId = passengerId
            };

            try
            {
                _luggageService.UpdateLuggage(newLuggage);
                return Ok(newLuggage);
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
        public IActionResult DeleteLuggage(int id)
        {
            try
            {
                _luggageService.DeleteLuggage(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
