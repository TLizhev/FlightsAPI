using FlightsAPI.Domain.Models;
using FlightsAPI.Repositories;

namespace FlightsAPI.Services
{
    public class PassengersService : IPassengersService
    {
        private readonly IPassengersRepository _passengersRepository;

        public PassengersService(IPassengersRepository passengersRepository)
        {
            _passengersRepository = passengersRepository;
        }

        public List<Passenger> GetPassengers()
        {
            return _passengersRepository.GetAll();
        }

        public Passenger GetPassenger(int id)
        {
            return _passengersRepository.GetById(id) ??
                   throw new InvalidOperationException("A passenger with this id does not exist.");
        }

        public async Task AddPassenger(Passenger newPassenger)
        {
            var passenger = _passengersRepository.GetById(newPassenger.Id);

            if (passenger is not null)
                throw new InvalidOperationException("This passenger already exists.");
     
            await _passengersRepository.AddAsync(newPassenger);
        }

        public void EditPassenger(Passenger newPassenger)
        {
            var passenger = _passengersRepository.GetById(newPassenger.Id);

            if (passenger is null)
                throw new InvalidOperationException("Passenger with this id does not exist.");

            passenger = newPassenger;

            _passengersRepository.Update(passenger);
        }

        public void DeletePassenger(int id)
        {
            var passenger = _passengersRepository.GetById(id);

            if (passenger is null)
                throw new InvalidOperationException("Passenger with this id does not exist.");

            _passengersRepository.Delete(passenger);
        }
    }
}
