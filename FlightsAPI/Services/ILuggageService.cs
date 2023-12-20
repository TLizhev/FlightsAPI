using FlightsAPI.Domain.Models;

namespace FlightsAPI.Services
{
    public interface ILuggageService
    {
        List<Luggage> GetLuggage();
        Luggage GetLuggage(int id);
        LuggageType GetMostPopularLuggage();
        Task AddLuggage(Luggage luggage);
        void UpdateLuggage(Luggage luggage);
        void DeleteLuggage(int id);
    }
}
