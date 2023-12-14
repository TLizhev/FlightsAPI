using FlightsAPI.Data.Models;
using FlightsAPI.Repositories;

namespace FlightsAPI.Services
{
    public class TicketsService : ITicketsService
    {
        private readonly ITicketsRepository _ticketRepository;
        private readonly IPassengersRepository _passengersRepository;

        public TicketsService(ITicketsRepository ticketRepository, IPassengersRepository passengersRepository)
        {
            _ticketRepository = ticketRepository;
            _passengersRepository = passengersRepository;
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
                var passenger = _passengersRepository.GetAll().FirstOrDefault(x => x.Id == keyValuePair.Value);
                if (passenger == null) continue;

                var fullName = passenger.FirstName + " " + passenger.LastName;
                results.Add(new FrequentFliersDto { FullName = fullName, Tickets = keyValuePair.Value });
            }

            return results;
        }
    }
}
