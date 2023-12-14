using FlightsAPI.Data.Models;
using FlightsAPI.Repositories;

namespace FlightsAPI.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketsRepository _ticketRepository;
        private readonly IPassengerRepository _passengerRepository;

        public TicketService(ITicketsRepository ticketRepository, IPassengerRepository passengerRepository)
        {
            _ticketRepository = ticketRepository;
            _passengerRepository = passengerRepository;
        }

        public List<Ticket> GetTickets()
        {
            return _ticketRepository.GetAll();
        }

        public Ticket GetTicket(int id)
        {
            return _ticketRepository.GetById(id);
        }

        public List<FrequentFliersDto> FrequentFliers()
        {
            var results = new List<FrequentFliersDto>();
            var passengerIds = _ticketRepository.GetAll().Select(x => x.PassengerId).ToList();
            var keyValuePairs = passengerIds.GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Select(y => y)
                    .Count()).Take(5).OrderByDescending(x => x.Value);

            foreach (var keyValuePair in keyValuePairs)
            {
                var passenger = _passengerRepository.GetAll().FirstOrDefault(x => x.Id == keyValuePair.Value);
                if (passenger == null) continue;

                var fullName = passenger.FirstName + " " + passenger.LastName;
                results.Add(new FrequentFliersDto { FullName = fullName, Tickets = keyValuePair.Value });
            }

            return results;
        }
    }
}
