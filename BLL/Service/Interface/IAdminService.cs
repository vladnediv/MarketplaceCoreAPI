using System.Linq.Expressions;
using BLL.Model;
using BLL.Model.DTO.Category;
using BLL.Model.DTO.Product;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestion;
using Domain.Model.Product;

namespace BLL.Service.Interface;

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
    
    
    //Category Management
    public Task<ServiceResponse> CreateCategoryAsync(CreateCategory createCategory);
    public Task<ServiceResponse> UpdateCategoryAsync(UpdateCategory updateCategory);
    public Task<ServiceResponse> DeleteCategoryAsync(int categoryId);
    public Task<ServiceResponse<CategoryDTO>> GetCategoryTreeAsync();
    public Task<ServiceResponse<CategoryDTO>> GetSubcategoriesAsync(int categoryId);

    //TODO Order management
}