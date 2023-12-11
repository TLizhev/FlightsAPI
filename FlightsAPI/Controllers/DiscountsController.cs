using FlightsAPI.Data;
using FlightsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers
{
    [ApiController]
    [Route(Endpoints.BaseDiscountsEndpoint)]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public int GetDiscount(int passengerId)
        {
            return _discountService.CalculateDiscount(passengerId);
        }
    }
}
