using System.Linq.Expressions;
using BLL.Model;
using BLL.Model.DTO.Product;
using Domain.Model.Product;

namespace BLL.Service.Interface.BasicInterface;

public interface IProductService : IAdvancedService<Product>
{
    public Task<ServiceResponse<ProductCardView>> GetProductCards(string searchQuery, Expression<Func<Product, bool>> predicate);
}