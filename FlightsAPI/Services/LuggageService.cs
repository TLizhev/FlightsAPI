using FlightsAPI.Data.Models;
using FlightsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Services
{
    public class LuggageService : ILuggageService
    {
        private readonly ILuggageRepository _luggageRepository;

        public LuggageService(ILuggageRepository luggageRepository)
        {
            _luggageRepository = luggageRepository;
        }

        public List<Luggage> GetLuggages()
        {
            return _luggageRepository.GetAll();
        }

        public Luggage GetLuggage(int id)
        {
            return _luggageRepository.GetById(id);
        }

        public LuggageType GetMostPopularLuggage()
        {
            var luggageTypes = _luggageRepository.GetAll().Select(x => x.LuggageTypeId).ToList();

            if (luggageTypes is { Count: <= 0 })
                throw new ArgumentException();

            var result = luggageTypes.GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Select(y => y)
                    .Count()).MaxBy(x => x.Value);

            if (result.Key is 0)
                throw new ArgumentException();

            return new LuggageType()
            {
                Id = result.Key,
                Type = _luggageRepository.GetLuggageTypes().First(x => x.Id == result.Key).Type
            };
        }

        public IActionResult UpdateLuggage(int id, int luggageTypeId, int passengerId)
        {
            var luggage = new Luggage
            {
                Id = id,
                LuggageTypeId = luggageTypeId,
                PassengerId = passengerId
            };

            _luggageRepository.Update(luggage);
            return new OkResult();
        }

        public IActionResult DeleteLuggage(int id)
        {
            _luggageRepository.Delete(id);
            return new OkResult();
        }

        public async Task<IActionResult> AddLuggage(int luggageTypeId, int passengerId)
        {
            if (luggageTypeId < 1 || passengerId < 1)
                return new BadRequestResult();

            var luggage = new Luggage
            {
                LuggageTypeId = luggageTypeId,
                PassengerId = passengerId
            };

            await _luggageRepository.AddAsync(luggage);
            return new OkResult();
        }
    }
}
