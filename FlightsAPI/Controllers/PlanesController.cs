using FlightsAPI.Application.Interfaces.Services;
using FlightsAPI.Data;
using FlightsAPI.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers;

[ApiController]
[Route(Endpoints.BasePlanesEndpoint)]
public class PlanesController : ControllerBase
{
    private readonly IPlanesService _planesService;

    public PlanesController(IPlanesService planesService)
    {
        _planesService = planesService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Plane>), 200)]
    [ProducesResponseType(204)]
    public IActionResult GetPlanes()
    {
        var planes = _planesService.GetPlanes();

        if (planes is { Count: 0 })
            return NoContent();

        return Ok(planes);
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(Plane), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetPlane(int id)
    {
        try
        {
            var plane = _planesService.GetPlane(id);
            return Ok(plane);
        }
        catch (Exception)
        {
            return NotFound("A plane with this id does not exist.");
        }
    }

    [HttpGet]
    [Route(Endpoints.SeatsEndpoint)]
    [ProducesResponseType(typeof(Plane), 200)]
    public IActionResult GetMostSeats()
    {
        return Ok(_planesService.GetMostSeats());
    }

    [HttpGet]
    [Route(Endpoints.RangeEndpoint)]
    [ProducesResponseType(typeof(Plane), 200)]
    public IActionResult GetMostRange()
    {
        return Ok(_planesService.GetBiggestRange());
    }

    [HttpPost]
    [ProducesResponseType(typeof(Plane), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddPlane(string name, int seats, int range)
    {
        try
        {
            var plane = new Plane
            {
                Name = name,
                Seats = seats,
                Range = range
            };

            await _planesService.AddPlane(plane);
            return CreatedAtRoute("GetPlane", new { planeId = plane.Id }, plane);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(Plane), 200)]
    [ProducesResponseType(404)]
    public IActionResult PatchPlane(int id, string name, int seats, int range)
    {
        var newPlane = new Plane
        {
            Id = id,
            Name = name,
            Seats = seats,
            Range = range
        };
        try
        {
            _planesService.EditPlane(newPlane);
            return Ok(newPlane);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(Plane), 204)]
    [ProducesResponseType(404)]
    public IActionResult DeletePlane(int id)
    {
        try
        {
            _planesService.DeletePlane(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}