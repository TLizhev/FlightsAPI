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

        public IActionResult UpdateLuggage(int id, int luggageTypeId, int passengerId)
        {
            try
            {
                var luggage = _luggageRepository.GetById(id);
                luggage.LuggageTypeId = luggageTypeId;
                luggage.PassengerId = passengerId;

                _luggageRepository.Update(luggage);
                return new OkResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public IActionResult DeleteLuggage(int id)
        {
            try
            {
                var luggage = _luggageRepository.GetById(id);

                _luggageRepository.Delete(luggage);
                return new OkResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
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
