using FlightsAPI.Data;
using FlightsAPI.Data.Models;
using FlightsAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FlightsAPI.Services
{
    public class FlightsService : IFlightsService
    {
        private readonly IFlightRepository _flightRepository;

        public FlightsService(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public Flight GetFlight(int id)
        {
            return _flightRepository.GetById(id);
        }

        public List<Flight> GetFlights()
        {
            return _flightRepository.GetAll();
        }

        public List<TopFiveDto> GetTopFiveFlightOrigins()
        {
            var origins = _flightRepository.GetAll().Select(x => x.Origin).ToList();

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

            if (_flightRepository.GetAll().Contains(flight)) return new BadRequestResult();
            _flightRepository.Add(flight);
            return new OkResult();
        }

        public async Task<IActionResult> EditFlight(int id,
            DateTime? arrivalTime,
            DateTime? departureTime,
            string origin,
            string destination,
            int planeId)
        {
            var flights = _flightRepository.GetAll();
            var flight = flights.FirstOrDefault(x => x.Id == id);

            if (!flights.Contains(flights.FirstOrDefault(x => x.Id == id)!)) 
                return new BadRequestResult();

            if (flight != null)
            {
                flight.Id = id;
                flight.DepartureTime = departureTime;
                flight.ArrivalTime = arrivalTime;
                flight.Origin = origin;
                flight.Destination = destination;
                flight.PlaneId = planeId;

                _flightRepository.Update(flight);
            }

            return new OkResult();
        }

        public async Task<IActionResult> DeleteFlight(int id)
        {
            var flights = _flightRepository.GetAll();
            var flight = flights.FirstOrDefault(x => x.Id == id);
            if (flight is null)
                return new NotFoundResult();
            flights.Remove(flight);

            return new OkResult();
        }

        public List<TopFiveDto> GetTopFiveFlightDestinations()
        {
            var destinations = _flightRepository.GetAll().Select(x => x.Destination).ToList();

            var flights = GetTopFiveFlights(destinations);

            return flights;
        }

        private static List<TopFiveDto> GetTopFiveFlights(List<string> flightList)
        {
            var result = new List<TopFiveDto>();

            var flights =
                flightList.GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Select(y => y)
                        .Count()).OrderByDescending(x => x.Value).Take(5).ToDictionary(x => x.Key, x => x.Value);

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