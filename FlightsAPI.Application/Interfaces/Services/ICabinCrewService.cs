using FlightsAPI.Domain.Models;

namespace FlightsAPI.Application.Interfaces.Services;

public interface ICabinCrewService
{
    List<CabinCrew> GetCabinCrew();
    CabinCrew GetCabinCrew(int id);
    Task AddCabinCrew(CabinCrew cabinCrew);

    void EditCabinCrew(CabinCrew cabinCrew);

    void DeleteCabinCrew(int id);
}