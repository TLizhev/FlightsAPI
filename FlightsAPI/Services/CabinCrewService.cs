using FlightsAPI.Application.Interfaces.Repositories;
using FlightsAPI.Application.Interfaces.Services;
using FlightsAPI.Domain.Models;

namespace FlightsAPI.Services
{
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
            return _cabinCrewRepository.AddAsync(cabinCrew);
        }

        public void EditCabinCrew(CabinCrew cabinCrew)
        {
            _cabinCrewRepository.Update(cabinCrew);
        }

        public void DeleteCabinCrew(int id)
        {
            var cabinCrew = _cabinCrewRepository.GetById(id);

            _cabinCrewRepository.Delete(cabinCrew);
        }
    }
}
