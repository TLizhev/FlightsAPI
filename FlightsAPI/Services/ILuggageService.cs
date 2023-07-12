using FlightsAPI.Data.Models;

namespace FlightsAPI.Services
{
    public interface ILuggageService
    {
        List<Luggage> GetLuggages();
        Luggage GetLuggage(int id);
        LuggageType GetMostPopularLuggage();
    }
}
