using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FlightsAPI.Controllers;
using FlightsAPI.Domain.Models;
using FlightsAPI.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FlightsAPITests.Controllers
{
    public class PassengersControllerTests
    {
        private readonly Mock<IPassengersService> _passengerService;
        private readonly PassengerController _sut;
        private readonly Fixture _fixture;

        public PassengersControllerTests()
        {
            _passengerService = new Mock<IPassengersService>();
            _sut = new PassengerController(_passengerService.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void GetPassengersReturnsOk()
        {
            // Arrange
            var passengers = _fixture.CreateMany<Passenger>().ToList();
            _passengerService.Setup(x => x.GetPassengers()).Returns(passengers);

            // Act
            var result = _sut.GetPassengerList();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetPassengersReturnsNoContentWhenListIsEmpty()
        {
            // Arrange
            _passengerService.Setup(x => x.GetPassengers()).Returns(new List<Passenger>());

            // Act
            var result = _sut.GetPassengerList();

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void GetPassengerByIdReturnsOk()
        {
            // Arrange
            var passenger = _fixture.Create<Passenger>();
            _passengerService.Setup(x => x.GetPassenger(passenger.Id)).Returns(passenger);

            // Act
            var result = _sut.GetPassenger(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetPassengerByIdReturnsNotFoundWhenPassengerDoesNotExist()
        {
            // Arrange
            _passengerService.Setup(x => x.GetPassenger(It.IsAny<int>())).Throws<InvalidOperationException>();

            // Act
            var result = _sut.GetPassenger(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task AddPassengerReturnsCreatedAtRoute()
        {
            // Arrange
            var passenger = _fixture.Create<Passenger>();

            // Act
            var result = await _sut.AddPassenger(
                passenger.FirstName, 
                passenger.LastName,
                passenger.Age,
                passenger.Address,
                passenger.PassportId
                );

            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>();
        }

        [Fact]
        public async Task AddReturnsNotFoundWhenPassengerDoesNotExist()
        {
            // Arrange
            _passengerService.Setup(x => x.AddPassenger(It.IsAny<Passenger>())).Throws<InvalidOperationException>();

            // Act
            var result = await _sut.AddPassenger(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void UpdatePassengerReturnsOk()
        {
            // Arrange
            var passenger = _fixture.Create<Passenger>();

            // Act
            var result = _sut.UpdatePassenger(
                passenger.Id, 
                passenger.FirstName, 
                passenger.LastName, 
                passenger.Age, 
                passenger.Address, 
                passenger.PassportId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void UpdateReturnsNotFoundWhenPassengerDoesNotExist()
        {
            // Arrange
            _passengerService.Setup(x => x.EditPassenger(It.IsAny<Passenger>())).Throws<InvalidDataException>();

            // Act
            var result = _sut.UpdatePassenger(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void DeletePassengerReturnsNoContent()
        {
            // Arrange
            var passenger = _fixture.Create<Passenger>();

            // Act
            var result = _sut.DeletePassenger(passenger.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void DeleteReturnsNotFoundWhenPassengerDoesNotExist()
        {
            // Arrange
            _passengerService.Setup(x => x.DeletePassenger(It.IsAny<int>())).Throws<InvalidOperationException>();

            // Act
            var result = _sut.DeletePassenger(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
