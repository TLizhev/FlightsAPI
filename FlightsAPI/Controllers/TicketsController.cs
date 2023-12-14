using FlightsAPI.Data;
using FlightsAPI.Data.Models;
using FlightsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers
{
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
        public List<Ticket> GetTickets()
        {
            return _ticketsService.GetTickets();
        }

        [HttpGet]
        [Route("{id:int}")]
        public Ticket GetTicket(int id)
        {
            return _ticketsService.GetTicket(id);
        }

        [HttpGet]
        [Route(Endpoints.FrequentFliersEndpoint)]
        public List<FrequentFliersDto> GetFrequentFliers()
        {
            return _ticketsService.FrequentFliers();
        }
    }
}
