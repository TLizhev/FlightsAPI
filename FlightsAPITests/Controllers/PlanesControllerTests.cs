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

namespace FlightsAPITests.Controllers;

public class PlanesControllerTests
{
    private readonly Mock<IPlanesService> _planesService;
    private readonly PlanesController _sut;
    private readonly Fixture _fixture;

    public PlanesControllerTests()
    {
        _planesService = new Mock<IPlanesService>();
        _fixture = new Fixture();
        _sut = new PlanesController(_planesService.Object);
    }

    [Fact]
    public void GetPlanesReturnsOk()
    {
        // Arrange
        var planes = _fixture.CreateMany<Plane>().ToList();
        _planesService.Setup(x => x.GetPlanes()).Returns(planes);

        // Act
        var result = _sut.GetPlanes();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public void GetPlanesReturnsNoContentWhenListIsEmpty()
    {
        // Arrange
        _planesService.Setup(x => x.GetPlanes()).Returns(new List<Plane>());

        // Act
        var result = _sut.GetPlanes();

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public void GetPlaneByIdReturnsOk()
    {
        // Arrange
        var plane = _fixture.Create<Plane>();
        _planesService.Setup(x => x.GetPlane(plane.Id)).Returns(plane);

        // Act
        var result = _sut.GetPlane(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public void GetPlaneByIdReturnsNotFoundWhenPlaneDoesNotExist()
    {
        // Arrange
        _planesService.Setup(x => x.GetPlane(It.IsAny<int>())).Throws<InvalidOperationException>();

        // Act
        var result = _sut.GetPlane(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task AddPlaneReturnsCreatedAtRoute()
    {
        // Arrange
        var plane = _fixture.Create<Plane>();

        // Act
        var result = await _sut.AddPlane(
            plane.Name,
            plane.Seats,
            plane.Range);

        // Assert
        result.Should().BeOfType<CreatedAtRouteResult>();
    }

    [Fact]
    public async Task AddReturnsNotFoundWhenPlaneDoesNotExist()
    {
        // Arrange
        _planesService.Setup(x => x.AddPlane(It.IsAny<Plane>())).Throws<InvalidOperationException>();

        // Act
        var result = await _sut.AddPlane(
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>());

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void PatchPlaneReturnsOk()
    {
        // Arrange
        var plane = _fixture.Create<Plane>();

        // Act
        var result = _sut.PatchPlane(
            plane.Id,
            plane.Name,
            plane.Seats,
            plane.Range);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public void UpdateReturnsNotFoundWhenPlaneDoesNotExist()
    {
        // Arrange
        _planesService.Setup(x => x.EditPlane(It.IsAny<Plane>())).Throws<InvalidDataException>();

        // Act
        var result = _sut.PatchPlane(
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public void DeletePlaneReturnsNoContent()
    {
        // Arrange
        var plane = _fixture.Create<Plane>();

        // Act
        var result = _sut.DeletePlane(plane.Id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public void DeleteReturnsNotFoundWhenPlaneDoesNotExist()
    {
        // Arrange
        _planesService.Setup(x => x.DeletePlane(It.IsAny<int>())).Throws<InvalidOperationException>();

        // Act
        var result = _sut.DeletePlane(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public void GetMostSeatsReturnsOk()
    {
        // Arrange
        var planes = _fixture.CreateMany<Plane>(2).ToList();
        planes[0].Seats = 100;
        planes[1].Seats = 200;
        _planesService.Setup(x => x.GetMostSeats()).Returns(planes[1]);

        // Act
        var result = _sut.GetMostSeats();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public void GetBiggestRangeReturnsOk()
    {
        // Arrange
        var planes = _fixture.CreateMany<Plane>(2).ToList();
        planes[0].Range = 100;
        planes[1].Range = 200;
        _planesService.Setup(x => x.GetBiggestRange()).Returns(planes[1]);

        // Act
        var result = _sut.GetMostRange();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
}
