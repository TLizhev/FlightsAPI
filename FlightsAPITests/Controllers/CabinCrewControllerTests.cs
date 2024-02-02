using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FlightsAPI.Application.Interfaces.Services;
using FlightsAPI.Controllers;
using FlightsAPI.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FlightsAPITests.Controllers
{
    public class CabinCrewControllerTests
    {
        private readonly Mock<ICabinCrewService> _service;
        private readonly CabinCrewController _sut;
        private readonly Fixture _fixture;

        public CabinCrewControllerTests()
        {
            _service = new Mock<ICabinCrewService>();
            _sut = new CabinCrewController(_service.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void GetCabinCrewReturnsOk()
        {
            // Arrange
            var cabinCrews = _fixture.CreateMany<CabinCrew>().ToList();
            _service.Setup(x => x.GetCabinCrew()).Returns(cabinCrews);

            // Act
            var result = _sut.GetCabinCrew();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetCabinCrewReturnsNoContentWhenListIsEmpty()
        {
            // Arrange
            _service.Setup(x => x.GetCabinCrew()).Returns(new List<CabinCrew>());

            // Act
            var result = _sut.GetCabinCrew();

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void GetCabinCrewByIdReturnsOk()
        {
            // Arrange
            var cabinCrew = _fixture.Create<CabinCrew>();
            _service.Setup(x => x.GetCabinCrew(cabinCrew.Id)).Returns(cabinCrew);

            // Act
            var result = _sut.GetCabinCrew(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetCabinCrewByIdReturnsNotFoundWhenCabinCrewDoesNotExist()
        {
            // Arrange
            _service.Setup(x => x.GetCabinCrew(It.IsAny<int>())).Throws<InvalidOperationException>();

            // Act
            var result = _sut.GetCabinCrew(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task AddCabinCrewReturnsCreatedAtRoute()
        {
            // Arrange
            var cabinCrew = _fixture.Create<CabinCrew>();

            // Act
            var result = await _sut.AddCabinCrew(cabinCrew.FirstName!, cabinCrew.LastName!, cabinCrew.Profession, cabinCrew.FlightId);

            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>();
        }

        [Fact]
        public async Task AddReturnsNotFoundWhenCabinCrewDoesNotExist()
        {
            // Arrange
            _service.Setup(x => x.AddCabinCrew(It.IsAny<CabinCrew>())).Throws<InvalidOperationException>();

            // Act
            var result = await _sut.AddCabinCrew(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Profession>(), It.IsAny<int>());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void EditCabinCrewReturnsOk()
        {
            // Arrange
            var cabinCrew = _fixture.Create<CabinCrew>();

            // Act
            var result = _sut.PatchCabinCrew(cabinCrew.Id, cabinCrew.FirstName!, cabinCrew.LastName!, cabinCrew.Profession, cabinCrew.FlightId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void EditReturnsNotFoundWhenCabinCrewDoesNotExist()
        {
            // Arrange
            _service.Setup(x => x.EditCabinCrew(It.IsAny<CabinCrew>())).Throws<InvalidDataException>();

            // Act
            var result = _sut.PatchCabinCrew(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Profession>(), It.IsAny<int>());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void DeleteCabinCrewReturnsNoContent()
        {
            // Arrange
            var cabinCrew = _fixture.Create<CabinCrew>();

            // Act
            var result = _sut.DeleteCabinCrew(cabinCrew.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void DeleteReturnsNotFoundWhenCabinCrewDoesNotExist()
        {
            // Arrange
            _service.Setup(x => x.DeleteCabinCrew(It.IsAny<int>())).Throws<InvalidOperationException>();

            // Act
            var result = _sut.DeleteCabinCrew(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
