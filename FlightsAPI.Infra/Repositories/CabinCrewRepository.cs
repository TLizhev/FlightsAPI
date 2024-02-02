using System.Diagnostics.CodeAnalysis;
using FlightsAPI.Application.Interfaces.Repositories;
using FlightsAPI.Domain.Models;

namespace FlightsAPI.Infra.Repositories;

[ExcludeFromCodeCoverage]
public class CabinCrewRepository : ICabinCrewRepository
{
    private readonly ApplicationDbContext _db;

    public CabinCrewRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public CabinCrew GetById(int id)
    {
        return _db.CabinCrews.FirstOrDefault(x => x.Id == id)!;
    }

    public List<CabinCrew> GetAll()
    {
        return _db.CabinCrews.ToList();
    }

    public async Task AddAsync(CabinCrew entity)
    {
        await _db.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public void Update(CabinCrew entity)
    {
        _db.Update(entity);
        _db.SaveChanges();
    }

    public void Delete(CabinCrew entity)
    {
        _db.Remove(entity);
        _db.SaveChanges();
    }
}