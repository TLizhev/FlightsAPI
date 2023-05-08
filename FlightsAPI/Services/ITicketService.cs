using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
{
    public interface ITicketService
    {
        public List<Ticket> GetTickets();
        public Ticket GetTicket(int id);
    }
}
