using BLL.Service.Model;
using DAL.Repository.DTO;
using Domain.Model.Product;

namespace BLL.Service.Interface;

public interface IProductService : IAdvancedService<Product>
{
    public Task<ServiceResponse<ProductCardView>> GetProductCards(string searchQuery);
}