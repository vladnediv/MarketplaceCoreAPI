using System.Linq.Expressions;
using BLL.Service.Model;

namespace BLL.Service.Interface;

public interface IAdvancedService<T> : IGenericService<T>  where T : class
{
    public Task<ServiceResponse<T>> GetAllAsync();
    public Task<ServiceResponse<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
    public Task<ServiceResponse<T>> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
}