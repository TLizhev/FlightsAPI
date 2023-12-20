using FlightsAPI.Domain.Models;
using FlightsAPI.Repositories;

namespace FlightsAPI.Services
{
    public class LuggageService : ILuggageService
    {
        private readonly ILuggageRepository _luggageRepository;

        public LuggageService(ILuggageRepository luggageRepository)
        {
            _luggageRepository = luggageRepository;
        }

        public List<Luggage> GetLuggage()
        {
            return _luggageRepository.GetAll();
        }

        public Luggage GetLuggage(int id)
        {
            return _luggageRepository.GetById(id) ?? throw new InvalidOperationException();
        }

        public LuggageType GetMostPopularLuggage()
        {
            var luggageTypes = _luggageRepository.GetAll().Select(x => x.LuggageTypeId).ToList();

            if (luggageTypes is { Count: <= 0 })
                throw new ArgumentException();

            var result = luggageTypes.GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Select(y => y)
                    .Count()).MaxBy(x => x.Value);

            return new LuggageType()
            {
                Id = result.Key,
                Type = _luggageRepository.GetLuggageTypes().First(x => x.Id == result.Key).Type
            };
        }

        public void UpdateLuggage(Luggage newLuggage)
        {
            var luggage = _luggageRepository.GetById(newLuggage.Id);
            if (luggage is null)
                throw new InvalidDataException("A luggage with this id does not exist.");

            luggage.LuggageTypeId = newLuggage.LuggageTypeId;
            luggage.PassengerId = newLuggage.PassengerId;

            _luggageRepository.Update(luggage);
        }

        public void DeleteLuggage(int id)
        {
            var luggage = _luggageRepository.GetById(id);
            if (luggage is null)
                throw new InvalidOperationException("A flight with this id does not exist.");

            _luggageRepository.Delete(luggage);
        }

        public async Task AddLuggage(Luggage luggage)
        {
            if (luggage.LuggageTypeId < 1 || luggage.PassengerId < 1)
                throw new InvalidDataException("Ids must be greater than 0");

            await _luggageRepository.AddAsync(luggage);
        }
    }
}
