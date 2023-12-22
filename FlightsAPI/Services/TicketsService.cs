using FlightsAPI.Application.Interfaces.Repositories;
using FlightsAPI.Application.Interfaces.Services;
using FlightsAPI.Domain.Models;

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
            return _ticketRepository.GetById(id) ??
                   throw new InvalidOperationException("A ticket with this id does not exist.");
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
                var passenger = _passengersRepository.GetAll().FirstOrDefault(x => x.Id == keyValuePair.Key);

                var fullName = passenger!.FirstName + " " + passenger.LastName;
                results.Add(new FrequentFliersDto { FullName = fullName, Tickets = keyValuePair.Value });
            }

            return results;
        }

        public async Task AddTicket(Ticket newTicket)
        {
            var ticket = _ticketRepository.GetById(newTicket.Id);

            if (ticket is not null)
                throw new InvalidOperationException("This passenger already exists.");

            await _ticketRepository.AddAsync(newTicket);
        }

        public void UpdateTicket(Ticket newTicket)
        {
            var ticket = _ticketRepository.GetById(newTicket.Id);

            if (ticket is null)
                throw new InvalidOperationException("Passenger with this id does not exist.");

            _ticketRepository.Update(newTicket);
        }

        public void DeleteTicket(int id)
        {
            var ticket = _ticketRepository.GetById(id);

            if (ticket is null)
                throw new InvalidOperationException("Passenger with this id does not exist.");

            _ticketRepository.Delete(ticket);
        }
    }
}
