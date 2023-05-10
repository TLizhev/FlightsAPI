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
            return _db.Tickets.FirstOrDefault(x => x.Id == id)!;
        }

        public List<FrequentFliersDto> FrequentFliers()
        {
            var results = new List<FrequentFliersDto>();
            var passengerIds = _db.Tickets.Select(x => x.PassengerId).ToList();
            var keyValuePairs = passengerIds.GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Select(y => y)
                    .Count()).Take(5).OrderByDescending(x => x.Value);

            foreach (var keyValuePair in keyValuePairs)
            {
                var passenger = _db.Passengers.FirstOrDefault(x => x.Id == keyValuePair.Value);
                if (passenger == null) continue;

                var fullName = passenger.FirstName + " " + passenger.LastName;
                results.Add(new FrequentFliersDto { FullName = fullName, Tickets = keyValuePair.Value });
            }

            return results;
        }
    }
}
