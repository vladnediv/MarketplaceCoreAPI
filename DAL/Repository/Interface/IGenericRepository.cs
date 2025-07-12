namespace DAL.Repository.Interface;

public interface IGenericRepository<T> where T : class
{
    //Basic CRUD operations contract
    public Task<T> GetByIdAsync(int id);
    public Task AddAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task DeleteAsync(T entity);
    public Task DeleteByIdAsync(int id);
}