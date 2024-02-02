using FlightsAPI.Application.Interfaces.Services;
using FlightsAPI.Data;
using FlightsAPI.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers;

[ApiController]
[Route(Endpoints.BaseCabinCrewRoute)]
public class CabinCrewController : ControllerBase
{
    private readonly ICabinCrewService _cabinCrewService;

    public CabinCrewController(ICabinCrewService cabinCrewService)
    {
        _cabinCrewService = cabinCrewService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CabinCrew>), 200)]
    [ProducesResponseType(204)]
    public IActionResult GetCabinCrew()
    {
        var cabinCrew = _cabinCrewService.GetCabinCrew();

        if (cabinCrew is { Count: 0 })
            return NoContent();

        return Ok(cabinCrew);
    }

    [HttpGet("{id:int}", Name = "GetCabinCrew")]
    [ProducesResponseType(typeof(CabinCrew), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetCabinCrew(int id)
    {
        try
        {
            var cabinCrew = _cabinCrewService.GetCabinCrew(id);
            return Ok(cabinCrew);
        }
        catch (Exception)
        {
            return NotFound("A Cabin Crew member with this id does not exist.");
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CabinCrew), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddCabinCrew(
        string firstName,
        string lastName,
        Profession profession,
        int flightId)
    {
        try
        {
            var cabinCrew = new CabinCrew
            {
                FirstName = firstName,
                LastName = lastName,
                Profession = profession,
                FlightId = flightId
            };

            await _cabinCrewService.AddCabinCrew(cabinCrew);
            return CreatedAtRoute("GetCabinCrew", new { id = cabinCrew.Id }, cabinCrew);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(CabinCrew), 200)]
    [ProducesResponseType(404)]
    public IActionResult PatchCabinCrew(
        int id,
        string firstName,
        string lastName,
        Profession profession,
        int flightId)
    {
        var cabinCrew = new CabinCrew
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            Profession = profession,
            FlightId = flightId
        };
        try
        {
            _cabinCrewService.EditCabinCrew(cabinCrew);
            return Ok(cabinCrew);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(CabinCrew), 204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCabinCrew(int id)
    {
        try
        {
            _cabinCrewService.DeleteCabinCrew(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}