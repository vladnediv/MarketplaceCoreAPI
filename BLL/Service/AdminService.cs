using System.Linq.Expressions;
using AutoMapper;
using BLL.Model;
using BLL.Model.DTO.Product;
using BLL.Service.Interface;
using BLL.Service.Interface.BasicInterface;
using BLL.Service.Model;
using BLL.Service.Model.DTO.Category;
using DAL.Repository.DTO;
using Domain.Model.Category;
using Domain.Model.Product;

namespace BLL.Service;

public class AdminService : IAdminService
{
    private readonly IProductService  _productService;
    private readonly IAdvancedService<DeliveryOption> _deliveryOptionService;
    private readonly ICategoryService _categoryService;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public AdminService(IProductService productService,
        IAdvancedService<DeliveryOption> deliveryOptionService,
        IFileService fileService,
        ICategoryService categoryService,
        IMapper mapper)
    {
        _productService = productService;
        _deliveryOptionService = deliveryOptionService;
        _fileService = fileService;
        _categoryService = categoryService;
        _mapper = mapper;
    }


    public async Task<ServiceResponse> EditProductApprovedStatusAsync(int productId, bool isApproved)
    {
        ServiceResponse serviceResponse = new ServiceResponse();
        
        var product = await _productService.GetAsync(productId);

        if (!product.IsSuccess)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = product.Message;
            
            return serviceResponse;
        }
        
        product.Entity.IsReviewed = true;
        product.Entity.IsApproved = isApproved;
        
        var updateRes = await _productService.UpdateAsync(product.Entity);
        
        if (!updateRes.IsSuccess)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = updateRes.Message;
            
            return serviceResponse;
        }
        
        serviceResponse.IsSuccess = true;
        return serviceResponse;
    }

    public async Task<ServiceResponse<AdminProductView>> GetProductsByParameterAsync(Expression<Func<Product, bool>> predicate)
    {
        //get the products by parameter
        ServiceResponse<Product> response = await _productService.GetAllAsync(predicate);
        ServiceResponse<AdminProductView> res = new ServiceResponse<AdminProductView>();
        if (response.IsSuccess)
        {
            //map the products to List<AdminProductView>
            List<AdminProductView> entities = response.Entities.Select(x => _mapper.Map<Product, AdminProductView>(x)).ToList();
            
            
            res.Entities = entities;
            res.IsSuccess = true;
        }
        else
        {
            res.IsSuccess = false;
            res.Message = response.Message;
        }
        
        return res;
    }

    public async Task<ServiceResponse> DeleteProductAsync(int productId)
    {
        ServiceResponse serviceResponse = new ServiceResponse();

        //get the product to get the picture locations
        var prodRes = await _productService.GetAsync(productId);
        
        //if couldnt find product, return error message
        if (!prodRes.IsSuccess)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = prodRes.Message;
            
            return serviceResponse;
        }
        //delete the product by ID
        var deleteRes = await _productService.DeleteByIdAsync(productId);

        //if couldnt delete product, return error message
        if (!deleteRes.IsSuccess)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = deleteRes.Message;
            
            return serviceResponse;
        }
        
        //delete product pictures
        foreach (var mediaFile in prodRes.Entity.MediaFiles)
        { 
            await _fileService.DeletePictureAsync(mediaFile.Url);
        }
        
        serviceResponse.IsSuccess = true;
        
        return serviceResponse;
    }

    public async Task<ServiceResponse> EditProductReviewApprovedStatusAsync(int reviewId, bool isApproved)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<ProductReviewDTO>> GetProductReviewsByParameterAsync(Expression<Func<ProductReview, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse> EditProductQuestionApprovedStatusAsync(int reviewId, bool isApproved)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<ProductQuestionDTO>> GetProductQuestionsByParameterAsync(Expression<Func<ProductQuestion, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse> AnswerProductQuestionAsync(int questionId, string answerText)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse> CreateDeliveryOptionAsync(string deliveryOption, decimal price)
    {
        var res = await _deliveryOptionService.CreateAsync(new DeliveryOption() { Name = deliveryOption, Price = price });

        return new ServiceResponse()
        {
            IsSuccess = res.IsSuccess,
            Message = res.Message
        };
    }

    public async Task<ServiceResponse<DeliveryOption>> GetAllDeliveryOptionsAsync()
    {
        ServiceResponse<DeliveryOption> res = await _deliveryOptionService.GetAllAsync();
        return res;
    }

    public async Task<ServiceResponse> CreateCategoryAsync(CRUDCategory createCategory)
    {
        var response = new ServiceResponse();

        if (createCategory == null || string.IsNullOrWhiteSpace(createCategory.Name))
        {
            response.IsSuccess = false;
            response.Message = BLL.Service.Model.Constants.ServiceResponseMessages.ArgumentIsNull(nameof(createCategory), nameof(CRUDCategory));
            return response;
        }

        var category = new Category
        {
            Name = createCategory.Name,
            ParentCategoryId = createCategory.ParentCategoryId == 0 ? null : createCategory.ParentCategoryId
        };

        var createRes = await _categoryService.CreateAsync(category);
        response.IsSuccess = createRes.IsSuccess;
        response.Message = createRes.Message;
        return response;
    }

    public async Task<ServiceResponse> UpdateCategoryAsync(CRUDCategory updateCategory)
    {
        var response = new ServiceResponse();

        // Validate payload
        if (updateCategory == null)
        {
            response.IsSuccess = false;
            response.Message = BLL.Service.Model.Constants.ServiceResponseMessages.ArgumentIsNull(nameof(updateCategory), nameof(CRUDCategory));
            return response;
        }

        if (!updateCategory.Id.HasValue)
        {
            response.IsSuccess = false;
            response.Message = BLL.Service.Model.Constants.ServiceResponseMessages.ArgumentIsNull(nameof(updateCategory.Id), nameof(CRUDCategory));
            return response;
        }

        if (updateCategory.Name == null)
        {
            response.IsSuccess = false;
            response.Message = BLL.Service.Model.Constants.ServiceResponseMessages.ArgumentIsNull(nameof(updateCategory.Name), nameof(CRUDCategory));
            return response;
        }

        var trimmedName = updateCategory.Name.Trim();
        if (trimmedName.Length == 0 || trimmedName != updateCategory.Name)
        {
            response.IsSuccess = false;
            response.Message = "Category name must not be empty and must not have leading or trailing spaces.";
            return response;
        }

        // Ensure category exists
        var existingRes = await _categoryService.GetAsync(updateCategory.Id.Value);
        if (!existingRes.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = existingRes.Message;
            return response;
        }

        // Parent validation
        if (updateCategory.ParentCategoryId.HasValue)
        {
            // Prevent setting itself as parent
            if (updateCategory.ParentCategoryId.Value == updateCategory.Id.Value)
            {
                response.IsSuccess = false;
                response.Message = "A category cannot be the parent of itself.";
                return response;
            }

            var parentRes = await _categoryService.GetAsync(updateCategory.ParentCategoryId.Value);
            if (!parentRes.IsSuccess)
            {
                response.IsSuccess = false;
                response.Message = parentRes.Message;
                return response;
            }
        }

        // Global name uniqueness check (exclude current)
        var duplicateRes = await _categoryService.FirstOrDefaultAsync(x => x.Id != updateCategory.Id.Value && x.Name == trimmedName);
        if (duplicateRes.IsSuccess && duplicateRes.Entity != null)
        {
            response.IsSuccess = false;
            response.Message = BLL.Service.Model.Constants.ServiceResponseMessages.AlreadyExists(trimmedName, nameof(Category));
            return response;
        }

        var toUpdate = existingRes.Entity;
        toUpdate.Name = trimmedName;
        toUpdate.ParentCategoryId = updateCategory.ParentCategoryId; // null makes it root

        var updateRes = await _categoryService.UpdateAsync(toUpdate);
        response.IsSuccess = updateRes.IsSuccess;
        response.Message = updateRes.Message;
        return response;
    }

    public async Task<ServiceResponse> DeleteCategoryAsync(int categoryId)
    {
        var res = await _categoryService.DeleteByIdAsync(categoryId);
        return new ServiceResponse
        {
            IsSuccess = res.IsSuccess,
            Message = res.Message
        };
    }

    public async Task<ServiceResponse<CategoryDTO>> GetCategoryTreeAsync()
    {
        var serviceRes = await _categoryService.GetCategoryTreeAsync();
        var response = new ServiceResponse<CategoryDTO>();
        if (serviceRes.IsSuccess)
        {
            response.IsSuccess = true;
            response.Entities = serviceRes.Entities.Select(c => _mapper.Map<CategoryDTO>(c)).ToList();
        }
        else
        {
            response.IsSuccess = false;
            response.Message = serviceRes.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<CategoryDTO>> GetSubcategoriesAsync(int categoryId)
    {
        var serviceRes = await _categoryService.GetSubcategoriesByParentIdAsync(categoryId);
        var response = new ServiceResponse<CategoryDTO>();
        if (serviceRes.IsSuccess)
        {
            response.IsSuccess = true;
            response.Entities = serviceRes.Entities.Select(c => _mapper.Map<CategoryDTO>(c)).ToList();
        }
        else
        {
            response.IsSuccess = false;
            response.Message = serviceRes.Message;
        }
        return response;
    }
}