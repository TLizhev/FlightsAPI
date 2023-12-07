using FlightsAPI.Data.Models;
using FlightsAPI.Repositories;

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
            return _flightRepository.GetById(id) ?? throw new InvalidOperationException();
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
        public List<TopFiveDto> TopFiveFlights(string direction)
        {
            return direction switch
            {
                "origin" => GetTopFiveFlightOrigins(),
                "destination" => GetTopFiveFlightDestinations(),
                _ => throw new ArgumentException("Please select origin or destination.")
            };
        }

        public async Task AddFlight(Flight flight)
        {
            if ((!flight.ArrivalTime.HasValue || !flight.DepartureTime.HasValue || string.IsNullOrWhiteSpace(flight.Origin)
                 || string.IsNullOrWhiteSpace(flight.Destination) || flight.PlaneId <= 0))
                throw new InvalidDataException();

            if (_flightRepository.GetAll().FirstOrDefault(x => x.Id == flight.Id) is not null)
                throw new InvalidOperationException("A flight with this id already exists.");

            await _flightRepository.AddAsync(flight);
        }

        public void EditFlight(Flight newFlight)
        {
            var flights = _flightRepository.GetAll();
            var flight = flights.FirstOrDefault(x => x.Id == newFlight.Id);

            if (flight is null)
                throw new InvalidDataException("A flight with this id does not exist.");

            flight.Id = newFlight.Id;
            flight.DepartureTime = newFlight.DepartureTime;
            flight.ArrivalTime = newFlight.ArrivalTime;
            flight.Origin = newFlight.Origin;
            flight.Destination = newFlight.Destination;
            flight.PlaneId = newFlight.PlaneId;

            _flightRepository.Update(flight);
        }

        public void DeleteFlight(int id)
        {
            var flights = _flightRepository.GetAll();
            var flight = flights.FirstOrDefault(x => x.Id == id);
            if (flight is null)
                throw new InvalidOperationException("A flight with this id does not exist.");

            _flightRepository.Delete(flight);
        }

        public List<TopFiveDto> GetTopFiveFlightDestinations()
        {
            var destinations = _flightRepository.GetAll().Select(x => x.Destination).ToList();

            var flights = GetTopFiveFlights(destinations);

            return flights;
        }

        private static List<TopFiveDto> GetTopFiveFlights(IEnumerable<string> flightList)
        {
            var flights =
                flightList.GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Select(y => y)
                        .Count()).OrderByDescending(x => x.Value).Take(5).ToDictionary(x => x.Key, x => x.Value);

            return flights
                .Select(flight => new TopFiveDto
                {
                    Name = flight.Key,
                    Number = flight.Value,
                })
                .ToList();
        }
    }
}