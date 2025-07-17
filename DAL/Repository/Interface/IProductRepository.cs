using Domain.Model.Product;

namespace DAL.Repository.Interface;

public interface IProductRepository : IAdvancedRepository<Product>
{
    public IQueryable<Product> GetQueryable();
}