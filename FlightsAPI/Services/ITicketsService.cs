using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
{
    public interface ITicketsService
    {
        public List<Ticket> GetTickets();
        public Ticket GetTicket(int id);
        public List<FrequentFliersDto> FrequentFliers();
    }
}
