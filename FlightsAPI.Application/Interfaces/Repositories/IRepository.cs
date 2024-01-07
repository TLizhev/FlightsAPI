namespace FlightsAPI.Application.Interfaces.Repositories;

public interface IRepository<T>
{
    T GetById(int id);
    List<T> GetAll();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}