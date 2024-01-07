using FlightsAPI.Application.Interfaces.Services;
using FlightsAPI.Data;
using FlightsAPI.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers;

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
    [ProducesResponseType(typeof(IEnumerable<Flight>), 200)]
    [ProducesResponseType(204)]
    public IActionResult GetFlights()
    {
        var flights = _flightService.GetFlights();

        if (flights is { Count: 0 })
            return NoContent();

        return Ok(flights);
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(Flight), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetFlight(int id)
    {
        try
        {
            var flight = _flightService.GetFlight(id);
            return Ok(flight);
        }
        catch (Exception)
        {
            return NotFound("A flight with this id does not exist.");
        }
    }

    [HttpGet]
    [Route(Endpoints.TopFlights)]
    [ProducesResponseType(typeof(IEnumerable<TopFiveDto>), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetTopFiveFlights(string direction)
    {
        try
        {
            var topFive = _flightService.TopFiveFlights(direction);
            return Ok(topFive);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }

    }

    [HttpPost]
    [ProducesResponseType(typeof(Flight), 201)]
    [ProducesResponseType(400)]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddFlight(
        string origin,
        string destination,
        DateTime? departureTime,
        DateTime? arrivalTime,
        int planeId)
    {
        try
        {
            var flight = new Flight
            {
                DepartureTime = departureTime,
                ArrivalTime = arrivalTime,
                Origin = origin,
                Destination = destination,
                PlaneId = planeId
            };

            await _flightService.AddFlight(flight);
            return CreatedAtRoute("GetFlight", new { flightId = flight.Id }, flight);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(Flight), 200)]
    [ProducesResponseType(404)]
    public IActionResult PatchFlight(
        int id,
        string origin,
        string destination,
        DateTime? departureTime,
        DateTime? arrivalTime,
        int planeId)
    {
        var newFlight = new Flight()
        {
            Destination = destination,
            ArrivalTime = arrivalTime,
            DepartureTime = departureTime,
            Id = id,
            Origin = origin,
            PlaneId = planeId
        };
        try
        {
            _flightService.EditFlight(newFlight);
            return Ok(newFlight);
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
    public IActionResult DeleteFlight(int id)
    {
        try
        {
            _flightService.DeleteFlight(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    [Route(Endpoints.LongestFlightEndpoint)]
    [ProducesResponseType(typeof(Flight), 200)]
    public IActionResult GetLongestFlight()
    {
        return Ok(_flightService.GetLongestFlight());
    }
}