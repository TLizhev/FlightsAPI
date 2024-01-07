using FlightsAPI.Domain.Models;

namespace FlightsAPI.Application.Interfaces.Services;

public interface ITicketsService
{
    List<Ticket> GetTickets();
    Ticket GetTicket(int id);
    List<FrequentFliersDto> FrequentFliers();
    Task AddTicket(Ticket newTicket);
    void UpdateTicket(Ticket ticket);
    void DeleteTicket(int id);
}