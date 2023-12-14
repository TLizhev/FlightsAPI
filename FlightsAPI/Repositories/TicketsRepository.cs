using FlightsAPI.Data;
using FlightsAPI.Data.Models;

namespace FlightsAPI.Repositories
{
    public class TicketsRepository : ITicketsRepository
    {
        private readonly ApplicationDbContext _db;

        public TicketsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Ticket GetById(int id)
        {
            return _db.Tickets.FirstOrDefault(x => x.Id == id)!;
        }

        public List<Ticket> GetAll()
        {
            return _db.Tickets.ToList();
        }

        public async Task AddAsync(Ticket entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public void Update(Ticket entity)
        {
            _db.Update(entity);
            _db.SaveChanges();
        }

        public void Delete(Ticket entity)
        {
            _db.Remove(entity);
            _db.SaveChanges();
        }
    }
}
