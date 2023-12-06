using AutoFixture;
using FlightsAPI.Data.Models;
using FlightsAPI.Repositories;
using FlightsAPI.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FlightsAPITests.Services
{
    public class FlightsServiceTests
    {
        private readonly FlightsService _sut;
        private readonly Mock<IFlightRepository> _flightsRepository;
        private readonly Fixture _fixture = new();

        public FlightsServiceTests()
        {
            _flightsRepository = new Mock<IFlightRepository>();
            _sut = new FlightsService(_flightsRepository.Object);
        }

        [Fact]
        public async Task AddFlightReturnsBadRequestWhenParametersAreInvalid()
        {
            // Arrange
            var flight = _fixture.Create<Flight>();
            flight.Destination = "";

            // Act
            var result = await _sut.AddFlight(
                flight.DepartureTime,
                flight.ArrivalTime,
                flight.Origin,
                flight.Destination,
                flight.PlaneId);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task AddFlightReturnsOkWhenParametersAreValid()
        {
            // Arrange
            var flight = _fixture.Create<Flight>();
            _flightsRepository.Setup(x => x.GetAll()).Returns(new List<Flight>());

            // Act
            var result = await _sut.AddFlight(
                flight.DepartureTime,
                flight.ArrivalTime,
                flight.Origin,
                flight.Destination,
                flight.PlaneId);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task AddFlightReturnsBadRequestWhenFlightAlreadyExists()
        {
            // Arrange
            var flight = new Flight
            {
                ArrivalTime = new DateTime(2023, 11, 30, 12, 00, 00),
                DepartureTime = new DateTime(2023, 11, 30, 10, 30, 00),
                Id = 0,
                Destination = "City A",
                Origin = "City B",
                PlaneId = 1
            };

            var flights = new List<Flight>() { flight, flight };
            _flightsRepository.Setup(x => x.GetAll()).Returns(flights);

            // Act
            var result = await _sut.AddFlight(
                flight.DepartureTime,
                flight.ArrivalTime,
                flight.Origin,
                flight.Destination,
                flight.PlaneId);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void GetFlightReturnsCorrectFlight()
        {
            var flight = _fixture.Create<Flight>();
            _flightsRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(flight);

            var result = _sut.GetFlight(flight.Id);

            result.Id.Should().Be(flight.Id);
        }

        [Fact]
        public void GetFlightThrowsWhenFlightDoesNotExist()
        {
            var flight = _fixture.Create<Flight>();
            _flightsRepository.Setup(x => x.GetById(It.IsAny<int>())).Throws<InvalidOperationException>();

            var result = () => _sut.GetFlight(flight.Id);

            result.Should().Throw<InvalidOperationException>();
        }
    }
}
