using FlightsAPI.Data;
using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
{
    public class LuggageService : ILuggageService
    {
        private readonly ApplicationDbContext db;

        public LuggageService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<Luggage> GetLuggages()
        {
            return db.Luggages.ToList();
        }

        public Luggage GetLuggage(int id)
        {
            return db.Luggages.FirstOrDefault(x => x.Id == id)!;
        }
    }
}
