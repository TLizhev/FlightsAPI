using FlightsAPI.Data.Models;

namespace FlightsAPI.Repositories
{
    public interface ILuggageRepository : IRepository<Luggage>
    {
        List<LuggageType> GetLuggageTypes();
    }
}
