using FlightsAPI.Data.Models;
using FlightsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers
{
    [ApiController]
    [Route("/api/luggage")]
    public class LuggageController : ControllerBase
    {
        private readonly ILuggageService _luggageService;

        public LuggageController(ILuggageService luggageService)
        {
            _luggageService = luggageService;
        }

        [HttpGet]
        public List<Luggage> GetLuggages()
        {
            return _luggageService.GetLuggages();
        }
        
        [HttpGet]
        [Route("{id:int}")]
        public Luggage GetLuggage(int id)
        {
            return _luggageService.GetLuggage(id);
        }

        [HttpGet]
        [Route("/popular")]
        public LuggageType GetPopularLuggage()
        {
            return _luggageService.GetMostPopularLuggage();
        }
    }
}
