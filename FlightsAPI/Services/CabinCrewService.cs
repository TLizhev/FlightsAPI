using FlightsAPI.Application.Interfaces.Repositories;
using FlightsAPI.Application.Interfaces.Services;
using FlightsAPI.Domain.Models;

namespace FlightsAPI.Services;

public class CabinCrewService : ICabinCrewService
{
    private readonly ICabinCrewRepository _cabinCrewRepository;

    public CabinCrewService(ICabinCrewRepository cabinCrewRepository)
    {
        _cabinCrewRepository = cabinCrewRepository;
    }

    public List<CabinCrew> GetCabinCrew()
    {
        return _cabinCrewRepository.GetAll();
    }

    public CabinCrew GetCabinCrew(int id)
    {
        return _cabinCrewRepository.GetById(id);
    }

    public Task AddCabinCrew(CabinCrew cabinCrew)
    {
        if (cabinCrew.FlightId < 1)
            throw new InvalidDataException("FlightId must be greater than 0.");
        return _cabinCrewRepository.AddAsync(cabinCrew);
    }

    public void EditCabinCrew(CabinCrew cabinCrew)
    {
        var newCabinCrew = _cabinCrewRepository.GetById(cabinCrew.Id);
        if (newCabinCrew is null)
            throw new InvalidDataException("A cabin crew member with this id does not exist.");
        _cabinCrewRepository.Update(cabinCrew);
    }

    public void DeleteCabinCrew(int id)
    {
        var cabinCrew = _cabinCrewRepository.GetById(id);
        if (cabinCrew is null)
            throw new InvalidOperationException("A cabin crew member with this id does not exist.");
        _cabinCrewRepository.Delete(cabinCrew);
    }
}