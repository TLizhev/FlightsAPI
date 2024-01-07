using FlightsAPI.Application.Interfaces.Services;
using FlightsAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers;

[ApiController]
[Route(Endpoints.BaseDiscountsEndpoint)]
public class DiscountsController : ControllerBase
{
    private readonly IDiscountsService _discountsService;

    public DiscountsController(IDiscountsService discountsService)
    {
        _discountsService = discountsService;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    public IActionResult GetDiscount(int passengerId)
    {
        var discount = _discountsService.CalculateDiscount(passengerId);
        return Ok(discount);
    }
}