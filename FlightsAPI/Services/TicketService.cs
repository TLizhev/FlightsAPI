using FlightsAPI.Data;
using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _db;

        public TicketService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Ticket> GetTickets()
        {
            return _db.Tickets.ToList();
        }

        public Ticket GetTicket(int id)
        {
            return _db.Tickets.FirstOrDefault(x =>x.Id == id);
        }
    }
}
