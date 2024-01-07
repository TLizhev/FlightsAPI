using FlightsAPI.Application.Interfaces.Repositories;
using FlightsAPI.Application.Interfaces.Services;
using FlightsAPI.Domain.Models;

namespace FlightsAPI.Services;

public class PlanesService : IPlanesService
{
    private readonly IPlanesRepository _planesRepository;

    public PlanesService(IPlanesRepository planesRepository)
    {
        _planesRepository = planesRepository;
    }

    public List<Plane> GetPlanes()
    {
        return _planesRepository.GetAll();
    }

    public Plane GetPlane(int id)
    {
        return _planesRepository.GetById(id) 
               ?? throw new InvalidOperationException("A plane with this id does not exist.");
    }

    public Plane GetMostSeats()
    {
        return _planesRepository.GetAll().MaxBy(x => x.Seats)!;
    }

    public Plane GetBiggestRange()
    {
        return _planesRepository.GetAll().MaxBy(x => x.Range)!;
    }

    public async Task AddPlane(Plane newPlane)
    {
        var plane = _planesRepository.GetById(newPlane.Id);

        if (plane is not null)
            throw new InvalidOperationException("A plane with this id already exists.");

        await _planesRepository.AddAsync(newPlane);
    }

    public void EditPlane(Plane newPlane)
    {
        var plane = _planesRepository.GetById(newPlane.Id);

        if (plane is null)
            throw new InvalidOperationException("A plane with this id does not exist.");

        _planesRepository.Update(newPlane);
    }

    public void DeletePlane(int id)
    {
        var plane = _planesRepository.GetById(id);

        if (plane is null)
            throw new InvalidOperationException("A plane with this id does not exist.");

        _planesRepository.Delete(plane);
    }
}