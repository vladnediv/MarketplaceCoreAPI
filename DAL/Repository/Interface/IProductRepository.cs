using DAL.Repository.DTO;
using Domain.Model.Product;

namespace DAL.Repository.Interface;

public interface IProductRepository : IAdvancedRepository<Product>
{
    public Task<IEnumerable<ProductCardDTO>> SearchProductsByParameter();
}