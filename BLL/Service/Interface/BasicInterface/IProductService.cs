using System.Linq.Expressions;
using BLL.Model;
using BLL.Model.DTO.Product;
using Domain.Model.Product;

namespace BLL.Service.Interface.BasicInterface;

public interface IProductService : IAdvancedService<Product>
{
    public Task<ServiceResponse<ProductCardView>> GetProductCards(string? searchQuery, int? categoryId, Expression<Func<Product, bool>>? predicate);
    public Task<ServiceResponse> ModifyProductStockAsync(bool decrease, int productId, int amount);
    public Task<ServiceResponse> CheckIfProductOnStock(int productId, int amount);
}