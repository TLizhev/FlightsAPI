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

        public Luggage GetMostPopularLuggage()
        {
            var luggageTypes = _db.Luggages.GroupBy(x => x.LuggageTypeId)
                .ToDictionary(x => x.Key, x => x.Select(y => y)
                    .Count());
            return _db.Luggages.FirstOrDefault(x => x.LuggageTypeId == luggageTypes.First().Key)!;
        }
    }
}
