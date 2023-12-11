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
        private readonly IPlaneService _planeService;

        public PlanesController(IPlaneService planeService)
        {
            _planeService = planeService;
        }

        [HttpGet]
        public List<Plane> GetPlanes()
        {
            return _planeService.GetPlanes();
        }

        [HttpGet]
        [Route("{id:int}")]
        public Plane GetPlane(int id)
        {
            return _planeService.GetPlane(id);
        }

        [HttpGet]
        [Route(Endpoints.SeatsEndpoint)]
        public Plane GetMostSeats()
        {
            return _planeService.GetMostSeats();
        }

        [HttpGet]
        [Route(Endpoints.RangeEndpoint)]
        public Plane GetMostRange()
        {
            return _planeService.GetBiggestRange();
        }
    }
}
