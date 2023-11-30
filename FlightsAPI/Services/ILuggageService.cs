using FlightsAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Services
{
    public interface ILuggageService
    {
        List<Luggage> GetLuggages();
        Luggage GetLuggage(int id);
        LuggageType GetMostPopularLuggage();
        IActionResult UpdateLuggage(int id, int luggageTypeId, int passengerId);
        IActionResult DeleteLuggage(int id);
    }
}
