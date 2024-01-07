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

public class TicketsControllerTests
{
    private readonly Mock<ITicketsService> _ticketsService;
    private readonly TicketsController _sut;
    private readonly Fixture _fixture;

    public TicketsControllerTests()
    {
        _ticketsService = new Mock<ITicketsService>();
        _sut = new TicketsController(_ticketsService.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public void GetTicketsReturnsOk()
    {
        // Arrange
        var tickets = _fixture.CreateMany<Ticket>().ToList();
        _ticketsService.Setup(x => x.GetTickets()).Returns(tickets);

        // Act
        var result = _sut.GetTickets();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public void GetTicketsReturnsNoContentWhenListIsEmpty()
    {
        // Arrange
        _ticketsService.Setup(x => x.GetTickets()).Returns(new List<Ticket>());

        // Act
        var result = _sut.GetTickets();

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public void GetTicketByIdReturnsOk()
    {
        // Arrange
        var ticket = _fixture.Create<Ticket>();
        _ticketsService.Setup(x => x.GetTicket(ticket.Id)).Returns(ticket);

        // Act
        var result = _sut.GetTicket(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public void GetTicketByIdReturnsNotFoundWhenTicketDoesNotExist()
    {
        // Arrange
        _ticketsService.Setup(x => x.GetTicket(It.IsAny<int>())).Throws<InvalidOperationException>();

        // Act
        var result = _sut.GetTicket(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task AddTicketReturnsCreatedAtRoute()
    {
        // Arrange
        var ticket = _fixture.Create<Ticket>();

        // Act
        var result = await _sut.AddTicket(
            ticket.PassengerId,
            ticket.FlightId,
            ticket.LuggageId,
            ticket.Price);

        // Assert
        result.Should().BeOfType<CreatedAtRouteResult>();
    }

    [Fact]
    public async Task AddReturnsNotFoundWhenTicketDoesNotExist()
    {
        // Arrange
        _ticketsService.Setup(x => x.AddTicket(It.IsAny<Ticket>())).Throws<InvalidOperationException>();

        // Act
        var result = await _sut.AddTicket(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<decimal>());

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void PatchTicketReturnsOk()
    {
        // Arrange
        var ticket = _fixture.Create<Ticket>();

        // Act
        var result = _sut.UpdateTicket(
            ticket.Id,
            ticket.PassengerId,
            ticket.FlightId,
            ticket.LuggageId,
            ticket.Price);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public void UpdateReturnsNotFoundWhenTicketDoesNotExist()
    {
        // Arrange
        _ticketsService.Setup(x => x.UpdateTicket(It.IsAny<Ticket>())).Throws<InvalidDataException>();

        // Act
        var result = _sut.UpdateTicket(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<decimal>());

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public void DeleteTicketReturnsNoContent()
    {
        // Arrange
        var ticket = _fixture.Create<Ticket>();

        // Act
        var result = _sut.DeleteTicket(ticket.Id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public void DeleteReturnsNotFoundWhenTicketDoesNotExist()
    {
        // Arrange
        _ticketsService.Setup(x => x.DeleteTicket(It.IsAny<int>())).Throws<InvalidOperationException>();

        // Act
        var result = _sut.DeleteTicket(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public void FrequentFliersReturnsOk()
    {
        // Arrange
        var tickets = _fixture.CreateMany<FrequentFliersDto>(2).ToList();

        _ticketsService.Setup(x => x.FrequentFliers()).Returns(tickets);

        // Act
        var result = _sut.GetFrequentFliers();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
}
