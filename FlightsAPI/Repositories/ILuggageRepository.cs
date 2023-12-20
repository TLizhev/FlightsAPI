using FlightsAPI.Domain.Models;

namespace FlightsAPI.Repositories
{
    public interface ILuggageRepository : IRepository<Luggage>
    {
        List<LuggageType> GetLuggageTypes();
    }
}
