using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FlightsAPI.Application.Interfaces.Repositories;
using FlightsAPI.Domain.Models;
using FlightsAPI.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace FlightsAPITests.Services;

public class PlanesServiceTests
{
    private readonly Mock<IPlanesRepository> _planesRepositoryMock;
    private readonly PlanesService _sut;
    private readonly Fixture _fixture;

    public PlanesServiceTests()
    {
        _planesRepositoryMock = new Mock<IPlanesRepository>();
        _sut = new PlanesService(_planesRepositoryMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public void GetPlanesReturnsResult()
    {
        // Arrange
        var planes = _fixture.CreateMany<Plane>().ToList();
        _planesRepositoryMock.Setup(x => x.GetAll()).Returns(planes);

        // Act
        var result = _sut.GetPlanes();

        // Assert
        result.Should().BeEquivalentTo(planes);
    }

    [Fact]
    public void GetPlaneByIdReturnsPlane()
    {
        // Arrange
        var plane = _fixture.Create<Plane>();
        _planesRepositoryMock.Setup(x => x.GetById(plane.Id)).Returns(plane);

        // Act
        var result = _sut.GetPlane(plane.Id);

        // Assert
        result.Should().Be(plane);
    }

    [Fact]
    public void GetPlaneByIdThrowsWhenPlaneDoesNotExist()
    {
        // Arrange
        _planesRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns<int>(id => id == 999 ? null : new Plane());

        // Act
        var result = () => _sut.GetPlane(999);

        // Assert
        result.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetMostSeatsReturnsCorrectResult()
    {
        // Arrange
        var plane1 = _fixture.Build<Plane>().With(x => x.Seats, 200).Create();
        var plane2 = _fixture.Build<Plane>().With(x => x.Seats, 300).Create();
        var planes = new List<Plane>() { plane1, plane2 };

        _planesRepositoryMock.Setup(x => x.GetAll()).Returns(planes);

        // Act
        var result = _sut.GetMostSeats();

        // Arrange
        result.Should().Be(plane2);
    }

    [Fact]
    public void GetBiggestRangeReturnsCorrectResult()
    {
        // Arrange
        var plane1 = _fixture.Build<Plane>().With(x => x.Range, 200).Create();
        var plane2 = _fixture.Build<Plane>().With(x => x.Range, 300).Create();
        var planes = new List<Plane>() { plane1, plane2 };

        _planesRepositoryMock.Setup(x => x.GetAll()).Returns(planes);

        // Act
        var result = _sut.GetBiggestRange();

        // Arrange
        result.Should().Be(plane2);
    }

    [Fact]
    public async Task AddPlaneThrowsWhenPassengerAlreadyExists()
    {
        // Arrange
        var planes = _fixture.CreateMany<Plane>().ToList();
        planes[0].Id = 1;

        var plane = _fixture.Build<Plane>().With(x => x.Id, 1).Create();
        _planesRepositoryMock.Setup(x => x.GetById(1)).Returns(plane);

        // Act
        var result = async () => await _sut.AddPlane(plane);

        // Assert
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task AddPlaneWorksWhenParametersAreValid()
    {
        // Arrange
        var plane = _fixture.Create<Plane>();

        // Act
        await _sut.AddPlane(plane);

        // Assert
        _planesRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Plane>()), Times.Once);
    }

    [Fact]
    public void EditPlaneThrowsWhenPassengerDoesNotExist()
    {
        // Arrange
        _planesRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns<int>(id => id == 999 ? null : new Plane());

        var plane = _fixture.Build<Plane>().With(x => x.Id, 999).Create();

        // Act
        var result = () => _sut.EditPlane(plane);

        // Assert
        result.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void EditPlaneWorksWhenParametersAreValid()
    {
        // Arrange
        var plane = _fixture.Create<Plane>();
        _planesRepositoryMock.Setup(x => x.GetById(plane.Id)).Returns(plane);

        // Act
        _sut.EditPlane(plane);

        // Assert
        _planesRepositoryMock.Verify(x => x.Update(It.IsAny<Plane>()), Times.Once);
    }

    [Fact]
    public void DeletePlaneThrowsWhenPassengerDoesNotExist()
    {
        // Arrange
        _planesRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns<int>(id => id == 999 ? null : new Plane());

        var plane = _fixture.Build<Plane>().With(x => x.Id, 999).Create();

        // Act
        var result = () => _sut.DeletePlane(plane.Id);

        // Assert
        result.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void DeletePlaneWorksWhenParametersAreValid()
    {
        // Arrange
        var plane = _fixture.Create<Plane>();
        _planesRepositoryMock.Setup(x => x.GetById(plane.Id)).Returns(plane);

        // Act
        _sut.DeletePlane(plane.Id);

        // Assert
        _planesRepositoryMock.Verify(x => x.Delete(It.IsAny<Plane>()), Times.Once);
    }
}
