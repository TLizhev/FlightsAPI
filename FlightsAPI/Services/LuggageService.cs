using FlightsAPI.Data;
using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
{
    public class LuggageService : ILuggageService
    {
        private readonly ApplicationDbContext _db;

        public LuggageService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Luggage> GetLuggages()
        {
            return _db.Luggages.ToList();
        }

        public Luggage GetLuggage(int id)
        {
            return _db.Luggages.FirstOrDefault(x => x.Id == id)!;
        }

        public LuggageType GetMostPopularLuggage()
        {
            var luggageTypes = _db.Luggages.Select(x => x.LuggageTypeId).ToList();

            var result = luggageTypes.GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Select(y => y)
                    .Count()).MaxBy(x => x.Value);

            return new LuggageType()
            {
                Id = result.Key,
                Type = _db.LuggageTypes.First(x => x.Id == result.Key).Type
            };
        }
    }
}
