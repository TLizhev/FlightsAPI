﻿using FlightsAPI.Data.Models;
using FlightsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Controllers
{
    [ApiController]
    [Route("/api/flights")]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightsService _flightService;

        public FlightsController(IFlightsService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public List<Flight> GetFlights()
        {
           return _flightService.GetFlights();
        } 
        
        [HttpGet]
        [Route("{id:int}")]
        public Flight GetFlight(int id)
        {
           return _flightService.GetFlight(id);
        }

        [HttpGet]
        [Route("/top")]
        public List<TopFiveDto> GetTop5Flights(string direction)
        {
            return _flightService.GetTopFiveFlights(direction);
        }

        [HttpPost]
        [Route("/add")]
        public async Task AddFlight(string origin,
            string destination,
            DateTime? departureTime,
            DateTime? arrivalTime,
            int planeId)
        {
            await _flightService.AddFlight(arrivalTime, departureTime, origin, destination, planeId);
        }

        [HttpPatch]
        [Route("/edit")]
        public async Task EditFlight(int id, 
            string origin,
            string destination,
            DateTime? departureTime,
            DateTime? arrivalTime,
            int planeId)
        {
            await _flightService.EditFlight(id,arrivalTime, departureTime, origin, destination, planeId);
        }

        [HttpDelete]
        [Route("/delete")]
        public async Task DeleteFlight(int id)
        {
            await _flightService.DeleteFlight(id);
        }
    }
}
