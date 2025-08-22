using BLL.Model;

namespace BLL.Service.Interface.BasicInterface;

public interface IGenericService<T> where T : class
{
    public Task<ServiceResponse<T>> GetAsync(int id);
    public Task<ServiceResponse<T>> CreateAsync(T entity);
    public Task<ServiceResponse<T>> UpdateAsync(T entity);
    public Task<ServiceResponse<T>> DeleteAsync(T entity);
    public Task<ServiceResponse<T>> DeleteByIdAsync(int id);
}