using System.Linq.Expressions;
using BLL.Model;
using BLL.Model.DTO.Product;
using BLL.Service.Model;
using BLL.Service.Model.DTO.Category;
using DAL.Repository.DTO;
using Domain.Model.Order;
using Domain.Model.Product;

namespace BLL.Service.Interface;

//TODO Think good about the functionality here
public interface IAdminService
{
    // Product management
    public Task<ServiceResponse> EditProductApprovedStatusAsync(int productId, bool isApproved);
    public Task<ServiceResponse<AdminProductView>> GetProductsByParameterAsync(Expression<Func<Product, bool>> predicate);
    public Task<ServiceResponse> DeleteProductAsync(int productId);
    
    // Question management
    public Task<ServiceResponse> EditProductQuestionApprovedStatusAsync(int reviewId, bool isApproved);
    public Task<ServiceResponse<ProductQuestionDTO>> GetProductQuestionsByParameterAsync(Expression<Func<ProductQuestion, bool>> predicate);
    public Task<ServiceResponse> AnswerProductQuestionAsync(int questionId, string answerText);

    //Delivery Management
    public Task<ServiceResponse> CreateDeliveryOptionAsync(string deliveryOption, decimal price);
    public Task<ServiceResponse<DeliveryOption>> GetAllDeliveryOptionsAsync();
    
    
    //Category Management
    public Task<ServiceResponse> CreateCategoryAsync(CRUDCategory createCategory);
    public Task<ServiceResponse> UpdateCategoryAsync(CRUDCategory updateCategory);
    public Task<ServiceResponse> DeleteCategoryAsync(int categoryId);
    public Task<ServiceResponse<CategoryDTO>> GetCategoryTreeAsync();
    public Task<ServiceResponse<CategoryDTO>> GetSubcategoriesAsync(int categoryId);

    //TODO Order management
}