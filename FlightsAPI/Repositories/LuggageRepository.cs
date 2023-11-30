﻿using FlightsAPI.Data;
using FlightsAPI.Data.Models;

namespace FlightsAPI.Repositories
{
    public class LuggageRepository : ILuggageRepository
    {
        private readonly ApplicationDbContext _db;

        public LuggageRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Luggage luggage)
        {
            await _db.Luggages.AddAsync(luggage);
            await _db.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            _db.Luggages.Remove(GetById(id));
        }

        public List<Luggage> GetAll()
        {
            return _db.Luggages.ToList();
        }

        public Luggage GetById(int id)
        {
            return _db.Luggages.First(x => x.Id == id);
        }

        public void Update(Luggage luggage)
        {
            _db.Luggages.Update(luggage);
            _db.SaveChanges();
        }

        public List<LuggageType> GetLuggageTypes()
        {
            return _db.LuggageTypes.ToList();
        }
    }
}
