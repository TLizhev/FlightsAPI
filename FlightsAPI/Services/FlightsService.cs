using System.Net;
using FlightsAPI.Data;
using FlightsAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Services
{
    public class FlightsService : IFlightsService
    {
        private readonly ApplicationDbContext _db;

        public FlightsService(ApplicationDbContext db)
        {
            this._db = db;
        }

        public Flight GetFlight(int id)
        {
            return _db.Flights.FirstOrDefault(x => x.Id == id)!;
        }

        public List<Flight> GetFlights()
        {
            return _db.Flights.ToList();
        }

        public List<TopFiveDto> GetTopFiveFlightOrigins()
        {
            var origins = _db.Flights.Select(x => x.Origin).ToList();

            var flights = GetTopFiveFlights(origins);

            return flights;
        }
        public List<TopFiveDto> GetTopFiveFlights(string direction)
        {
            return direction switch
            {
                "origin" => GetTopFiveFlightOrigins(),
                "destination" => GetTopFiveFlightDestinations(),
                _ => throw new ArgumentException("Please select origin or destination.")
            };
        }

        public async Task<IActionResult> AddFlight(DateTime? arrivalTime,
            DateTime? departureTime,
            string origin,
            string destination,
            int planeId)
        {
            var flight = new Flight
            {
                DepartureTime = departureTime,
                ArrivalTime = arrivalTime,
                Origin = origin,
                Destination = destination,
                PlaneId = planeId
            };

            if (_db.Flights.Contains(flight)) return new BadRequestResult();
            _db.Flights.Add(flight);
            await _db.SaveChangesAsync();
            return new OkResult();
        }

        public List<TopFiveDto> GetTopFiveFlightDestinations()
        {
            var destinations = _db.Flights.Select(x => x.Destination).ToList();

            var flights = GetTopFiveFlights(destinations);

            return flights;
        }

        private List<TopFiveDto> GetTopFiveFlights(List<string> flightList)
        {
            var result = new List<TopFiveDto>();

            var flights =
                flightList.GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Select(y => y)
                        .Count()).OrderByDescending(x => x.Value).Take(5).ToDictionary(x=>x.Key, x=>x.Value);

            foreach (var flight in flights)
            {
                var dto = new TopFiveDto
                {
                    Name = flight.Key,
                    Number = flight.Value,
                };
                result.Add(dto);
            }

            return result;
        }
    }
}