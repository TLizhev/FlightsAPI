using FlightsAPI.Data;
using FlightsAPI.Data.Models;
using FlightsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers
{
    [ApiController]
    [Route(Endpoints.BasePlanesEndpoint)]
    public class PlanesController : ControllerBase
    {
        private readonly IPlanesService _planesService;

        public PlanesController(IPlanesService planesService)
        {
            _planesService = planesService;
        }

        [HttpGet]
        public List<Plane> GetPlanes()
        {
            return _planesService.GetPlanes();
        }

        [HttpGet]
        [Route("{id:int}")]
        public Plane GetPlane(int id)
        {
            return _planesService.GetPlane(id);
        }

        [HttpGet]
        [Route(Endpoints.SeatsEndpoint)]
        public Plane GetMostSeats()
        {
            return _planesService.GetMostSeats();
        }

        [HttpGet]
        [Route(Endpoints.RangeEndpoint)]
        public Plane GetMostRange()
        {
            return _planesService.GetBiggestRange();
        }
    }
}
