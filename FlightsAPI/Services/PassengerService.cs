using FlightsAPI.Data.Models;
using FlightsAPI.Repositories;

namespace FlightsAPI.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;

        public PassengerService(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public List<Passenger> GetPassengers()
        {
            return _passengerRepository.GetAll();
        }

        public Passenger GetPassenger(int id)
        {
            return _passengerRepository.GetById(id) ??
                   throw new InvalidOperationException("A passenger with this id does not exist.");
        }

        public async Task AddPassenger(Passenger passenger)
        {
            await _passengerRepository.AddAsync(passenger);
        }

        public void EditPassenger(Passenger newPassenger)
        {
            var passenger = _passengerRepository.GetById(newPassenger.Id);

            if (passenger is null)
                throw new InvalidOperationException("Passenger with this id does not exist.");

            passenger = newPassenger;

            _passengerRepository.Update(passenger);
        }

        public void DeletePassenger(int id)
        {
            var passenger = _passengerRepository.GetById(id);

            if (passenger is null)
                throw new InvalidOperationException("Passenger with this id does not exist.");

            _passengerRepository.Delete(passenger);
        }
    }
}
