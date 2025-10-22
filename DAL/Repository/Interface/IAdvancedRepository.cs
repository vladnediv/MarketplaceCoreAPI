using System.Linq.Expressions;

namespace DAL.Repository.Interface;

public interface IAdvancedRepository<T> : IGenericRepository<T> where T : class
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
    public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
}