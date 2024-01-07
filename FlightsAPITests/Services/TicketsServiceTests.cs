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

public class TicketsServiceTests
{
    private readonly Mock<ITicketsRepository> _ticketsRepositoryMock;
    private readonly Mock<IPassengersRepository> _passengersRepositoryMock;
    private readonly TicketsService _sut;
    private readonly Fixture _fixture;

    public TicketsServiceTests()
    {
        _ticketsRepositoryMock = new Mock<ITicketsRepository>();
        _passengersRepositoryMock = new Mock<IPassengersRepository>();
        _fixture = new Fixture();

        _sut = new TicketsService(
            _ticketsRepositoryMock.Object,
            _passengersRepositoryMock.Object);
    }

    [Fact]
    public void GetTicketsReturnsResult()
    {
        // Arrange
        var tickets = _fixture.CreateMany<Ticket>().ToList();
        _ticketsRepositoryMock.Setup(x => x.GetAll()).Returns(tickets);

        // Act
        var result = _sut.GetTickets();

        // Assert
        result.Should().BeEquivalentTo(tickets);
    }

    [Fact]
    public void GetTicketByIdReturnsTicket()
    {
        // Arrange
        var ticket = _fixture.Create<Ticket>();
        _ticketsRepositoryMock.Setup(x => x.GetById(ticket.Id)).Returns(ticket);

        // Act
        var result = _sut.GetTicket(ticket.Id);

        // Assert
        result.Should().Be(ticket);
    }

    [Fact]
    public void GetTicketByIdThrowsWhenTicketDoesNotExist()
    {
        // Arrange
        _ticketsRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns<int>(id => id == 999 ? null : new Ticket());

        // Act
        var result = () => _sut.GetTicket(999);

        // Assert
        result.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task AddTicketThrowsWhenPassengerAlreadyExists()
    {
        // Arrange
        var tickets = _fixture.CreateMany<Ticket>().ToList();
        tickets[0].Id = 1;

        var ticket = _fixture.Build<Ticket>().With(x => x.Id, 1).Create();
        _ticketsRepositoryMock.Setup(x => x.GetById(1)).Returns(ticket);

        // Act
        var result = async () => await _sut.AddTicket(ticket);

        // Assert
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task AddTicketWorksWhenParametersAreValid()
    {
        // Arrange
        var ticket = _fixture.Create<Ticket>();

        // Act
        await _sut.AddTicket(ticket);

        // Assert
        _ticketsRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Ticket>()), Times.Once);
    }

    [Fact]
    public void UpdateTicketThrowsWhenPassengerDoesNotExist()
    {
        // Arrange
        _ticketsRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns<int>(id => id == 999 ? null : new Ticket());

        var ticket = _fixture.Build<Ticket>().With(x => x.Id, 999).Create();

        // Act
        var result = () => _sut.UpdateTicket(ticket);

        // Assert
        result.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void UpdateTicketWorksWhenParametersAreValid()
    {
        // Arrange
        var ticket = _fixture.Create<Ticket>();
        _ticketsRepositoryMock.Setup(x => x.GetById(ticket.Id)).Returns(ticket);

        // Act
        _sut.UpdateTicket(ticket);

        // Assert
        _ticketsRepositoryMock.Verify(x => x.Update(It.IsAny<Ticket>()), Times.Once);
    }

    [Fact]
    public void DeleteTicketThrowsWhenPassengerDoesNotExist()
    {
        // Arrange
        _ticketsRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns<int>(id => id == 999 ? null : new Ticket());

        var ticket = _fixture.Build<Ticket>().With(x => x.Id, 999).Create();

        // Act
        var result = () => _sut.DeleteTicket(ticket.Id);

        // Assert
        result.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void DeleteTicketWorksWhenParametersAreValid()
    {
        // Arrange
        var ticket = _fixture.Create<Ticket>();
        _ticketsRepositoryMock.Setup(x => x.GetById(ticket.Id)).Returns(ticket);

        // Act
        _sut.DeleteTicket(ticket.Id);

        // Assert
        _ticketsRepositoryMock.Verify(x => x.Delete(It.IsAny<Ticket>()), Times.Once);
    }

    [Fact]
    public void FrequentFliersReturnsResult()
    {
        // Arrange
        var tickets = _fixture.CreateMany<Ticket>().ToList();
        tickets[0].PassengerId = 2;
        tickets[1].PassengerId = 2;
        tickets[2].PassengerId = 3;

        var passengers = _fixture.CreateMany<Passenger>().ToList();
        passengers[0].Id = 1;
        passengers[1].Id = 2;
        passengers[2].Id = 3;

        _ticketsRepositoryMock.Setup(x => x.GetAll()).Returns(tickets);
        _passengersRepositoryMock.Setup(x => x.GetAll()).Returns(passengers);

        var expected = new List<FrequentFliersDto>()
            {
                new()
                {
                    FullName = passengers[1].FirstName + " " + passengers[1].LastName,
                    Tickets = 2
                },
                new()
                {
                    FullName = passengers[2].FirstName + " " + passengers[2].LastName,
                    Tickets = 1
                },
            };

        // Act
        var result = _sut.FrequentFliers();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
