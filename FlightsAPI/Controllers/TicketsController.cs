using FlightsAPI.Application.Interfaces.Services;
using FlightsAPI.Data;
using FlightsAPI.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers;

[ApiController]
[Route(Endpoints.BaseTicketsEndpoint)]
public class TicketsController : ControllerBase
{
    private readonly ITicketsService _ticketsService;

    public TicketsController(ITicketsService ticketsService)
    {
        _ticketsService = ticketsService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Ticket>), 200)]
    [ProducesResponseType(204)]
    public IActionResult GetTickets()
    {
        var tickets = _ticketsService.GetTickets();

        if (tickets is { Count: 0 })
            return NoContent();

        return Ok(tickets);
    }

    [HttpGet("{id:int}", Name = "GetTicket")]
    [ProducesResponseType(typeof(Ticket), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetTicket(int id)
    {
        try
        {
            var ticket = _ticketsService.GetTicket(id);
            return Ok(ticket);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    [Route(Endpoints.FrequentFliersEndpoint)]
    [ProducesResponseType(typeof(IEnumerable<FrequentFliersDto>), 200)]
    public IActionResult GetFrequentFliers()
    {
        return Ok(_ticketsService.FrequentFliers());
    }

    [HttpPost]
    [ProducesResponseType(typeof(Ticket), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddTicket(
        int passengerId,
        int flightId,
        int luggageId,
        decimal price)
    {
        try
        {
            var ticket = new Ticket
            {
                PassengerId = passengerId,
                FlightId = flightId,
                LuggageId = luggageId,
                Price = price
            };

            await _ticketsService.AddTicket(ticket);
            return CreatedAtRoute("GetTicket", new { id = ticket.Id }, ticket);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(Ticket), 200)]
    [ProducesResponseType(404)]
    public IActionResult UpdateTicket(
        int id,
        int passengerId,
        int flightId,
        int luggageId,
        decimal price)
    {
        var ticket = new Ticket()
        {
            Id = id,
            PassengerId = passengerId,
            FlightId = flightId,
            LuggageId = luggageId,
            Price = price
        };

        try
        {
            _ticketsService.UpdateTicket(ticket);
            return Ok(ticket);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType(typeof(Ticket), 204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteTicket(int id)
    {
        try
        {
            _ticketsService.DeleteTicket(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}