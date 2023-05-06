using FlightsAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers
{
    public class FlightsController : ControllerBase
    {
        public List<Flight> GetFlights()
        {
            return new();
        }
    }
}
