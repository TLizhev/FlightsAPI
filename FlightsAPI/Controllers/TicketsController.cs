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
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public List<Ticket> GetTickets()
        {
            return _ticketService.GetTickets();
        }

        [HttpGet]
        [Route("{id:int}")]
        public Ticket GetTicket(int id)
        {
            return _ticketService.GetTicket(id);
        }

        [HttpGet]
        [Route(Endpoints.FrequentFliersEndpoint)]
        public List<FrequentFliersDto> GetFrequentFliers()
        {
            return _ticketService.FrequentFliers();
        }
    }
}
