using FlightsAPI.Data.Models;
using FlightsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers
{
    [ApiController]
    [Route("api/planes")]
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
    }
}
