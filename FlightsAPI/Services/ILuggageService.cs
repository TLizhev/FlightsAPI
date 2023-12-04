using FlightsAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Services
{
    public interface ILuggageService
    {
        List<Luggage> GetLuggages();
        Luggage GetLuggage(int id);
        LuggageType GetMostPopularLuggage();
        Task<IActionResult> AddLuggage(int luggageTypeId, int passengerId);
        IActionResult UpdateLuggage(int id, int luggageTypeId, int passengerId);
        IActionResult DeleteLuggage(int id);
    }
}
