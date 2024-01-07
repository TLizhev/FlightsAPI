using FlightsAPI.Domain.Models;

namespace FlightsAPI.Application.Interfaces.Repositories;

public interface ILuggageRepository : IRepository<Luggage>
{
    List<LuggageType> GetLuggageTypes();
}