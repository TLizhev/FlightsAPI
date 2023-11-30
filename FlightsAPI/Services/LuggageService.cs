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

            var result = luggageTypes.GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Select(y => y)
                    .Count()).MaxBy(x => x.Value);

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
    }
}
