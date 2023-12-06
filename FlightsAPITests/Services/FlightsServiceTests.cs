using AutoFixture;
using FlightsAPI.Data.Models;
using FlightsAPI.Repositories;
using FlightsAPI.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            // Arrange
            var flight = _fixture.Create<Flight>();
            _flightsRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(flight);

            // Act
            var result = _sut.GetFlight(flight.Id);

            // Assert
            result.Id.Should().Be(flight.Id);
        }

        [Fact]
        public void GetFlightThrowsWhenFlightDoesNotExist()
        {
            // Arrange
            var flight = _fixture.Create<Flight>();
            _flightsRepository.Setup(x => x.GetById(It.IsAny<int>())).Throws<InvalidOperationException>();

            // Act
            var result = () => _sut.GetFlight(flight.Id);

            // Assert
            result.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GetFlightsReturnsFlights()
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>().ToList();
            _flightsRepository.Setup(x => x.GetAll()).Returns(flights);

            // Act
            var result = _sut.GetFlights();

            // Assert
            result.Should().BeEquivalentTo(flights);
            result.Count.Should().Be(3);
        }

        [Fact]
        public void EditFlightReturnsOkWhenFlightExists()
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>().ToList();
            var flight = flights[0];
            flight.Id = 5;

            _flightsRepository.Setup(x => x.GetAll()).Returns(flights);

            // Act
            var result = _sut.EditFlight(
                flight.Id,
                flight.ArrivalTime,
                flight.DepartureTime,
                flight.Origin,
                flight.Destination,
                It.IsAny<int>());

            // Assert
            result.Should().BeOfType<OkResult>();
            _flightsRepository.Verify(x => x.Update(flight), Times.Once);
        }

        [Fact]
        public void EditFlightReturnsBadRequestWhenFlightDoesNotExist()
        {
            // Arrange
            _flightsRepository.Setup(x => x.GetAll()).Returns(new List<Flight>());

            // Act
            var result = _sut.EditFlight(
                It.IsAny<int>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>());

            result.Should().BeOfType<BadRequestResult>();
            _flightsRepository.Verify(x => x.Update(It.IsAny<Flight>()), Times.Never);
        }

        [Fact]
        public void DeleteFlightReturnsOkWhenFlightExists()
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>().ToList();
            var flight = flights[0];
            flight.Id = 5;

            _flightsRepository.Setup(x => x.GetAll()).Returns(flights);

            // Act
            var result = _sut.DeleteFlight(flight.Id);

            // Assert
            result.Should().BeOfType<OkResult>();
            _flightsRepository.Verify(x => x.Delete(flight), Times.Once);
        }

        [Fact]
        public void DeleteFlightReturnsNotFoundWhenFlightDoesNotExist()
        {
            // Arrange
            _flightsRepository.Setup(x => x.GetAll()).Returns(new List<Flight>());

            // Act
            var result = _sut.DeleteFlight(It.IsAny<int>());

            result.Should().BeOfType<NotFoundResult>();
            _flightsRepository.Verify(x => x.Delete(It.IsAny<Flight>()), Times.Never);
        }

        [Fact]
        public void GetTopFiveOriginsReturnsCorrectResult()
        {
            // Arrange
            var liverpoolFlights = _fixture.Build<Flight>().With(x => x.Origin, "Liverpool").CreateMany(2).ToList();
            var manchesterFlights = _fixture.Build<Flight>().With(x => x.Origin, "Manchester").CreateMany(2).ToList();
            var varnaFlights = _fixture.Build<Flight>().With(x => x.Origin, "Varna").CreateMany(5).ToList();
            var sofiaFlights = _fixture.Build<Flight>().With(x => x.Origin, "Sofia").CreateMany(4).ToList();
            var londonFlights = _fixture.Build<Flight>().With(x => x.Origin, "London").CreateMany(3).ToList();
            var lutonFlights = _fixture.Build<Flight>().With(x => x.Origin, "Luton").CreateMany(2).ToList();

            var flights = varnaFlights
                .Concat(sofiaFlights)
                .Concat(londonFlights)
                .Concat(liverpoolFlights)
                .Concat(manchesterFlights)
                .Concat(lutonFlights)
                .ToList();

            _flightsRepository.Setup(x => x.GetAll()).Returns(flights);

            var expected = new List<TopFiveDto>()
            {
                new()
                {
                    Name = "Varna",
                    Number = 5
                },
                new()
                {
                    Name = "Sofia",
                    Number = 4
                },
                new()
                {
                    Name = "London",
                    Number = 3
                },
                new()
                {
                    Name = "Liverpool",
                    Number = 2
                },
                new()
                {
                    Name = "Manchester",
                    Number = 2
                },
            };

            // Act
            var result = _sut.GetTopFiveFlightOrigins();

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetTopFiveDestinationsReturnsCorrectResult()
        {
            // Arrange
            var liverpoolFlights = _fixture.Build<Flight>().With(x => x.Destination, "Liverpool").CreateMany(2).ToList();
            var manchesterFlights = _fixture.Build<Flight>().With(x => x.Destination, "Manchester").CreateMany(2).ToList();
            var varnaFlights = _fixture.Build<Flight>().With(x => x.Destination, "Varna").CreateMany(5).ToList();
            var sofiaFlights = _fixture.Build<Flight>().With(x => x.Destination, "Sofia").CreateMany(4).ToList();
            var londonFlights = _fixture.Build<Flight>().With(x => x.Destination, "London").CreateMany(3).ToList();
            var lutonFlights = _fixture.Build<Flight>().With(x => x.Destination, "Luton").CreateMany(2).ToList();

            var flights = varnaFlights
                .Concat(sofiaFlights)
                .Concat(londonFlights)
                .Concat(liverpoolFlights)
                .Concat(manchesterFlights)
                .Concat(lutonFlights)
                .ToList();

            _flightsRepository.Setup(x => x.GetAll()).Returns(flights);

            var expected = new List<TopFiveDto>()
            {
                new()
                {
                    Name = "Varna",
                    Number = 5
                },
                new()
                {
                    Name = "Sofia",
                    Number = 4
                },
                new()
                {
                    Name = "London",
                    Number = 3
                },
                new()
                {
                    Name = "Liverpool",
                    Number = 2
                },
                new()
                {
                    Name = "Manchester",
                    Number = 2
                },
            };

            // Act
            var result = _sut.GetTopFiveFlightDestinations();

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetTopFiveThrowsWhenDirectionIsNotSupported()
        {
            // Act
            var result = () => _sut.TopFiveFlights("bla");

            // Assert
            result.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetTopFiveReturnsOrigins()
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>().ToList();
            _flightsRepository.Setup(x => x.GetAll()).Returns(flights);

            // Act
            _ = _sut.TopFiveFlights("origin");

            // Assert
            _flightsRepository.Verify(x=>x.GetAll(), Times.Once);
        }

        [Fact]
        public void GetTopFiveReturnsDestinations()
        {
            // Arrange
            var flights = _fixture.CreateMany<Flight>().ToList();
            _flightsRepository.Setup(x => x.GetAll()).Returns(flights);

            // Act
            _ = _sut.TopFiveFlights("destination");

            // Assert
            _flightsRepository.Verify(x => x.GetAll(), Times.Once);
        }
    }
}
