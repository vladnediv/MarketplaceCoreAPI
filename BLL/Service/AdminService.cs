using System.Linq.Expressions;
using AutoMapper;
using BLL.Model;
using BLL.Model.Constants;
using BLL.Model.DTO.Category;
using BLL.Model.DTO.Product;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestion;
using BLL.Model.DTO.Product.IncludedModels.ProductReview;
using BLL.Service.Interface;
using BLL.Service.Interface.BasicInterface;
using Domain.Model.Category;
using Domain.Model.Product;

namespace BLL.Service;

public class AdminService : IAdminService
{
    private readonly IProductService  _productService;
    private readonly IAdvancedService<DeliveryOption> _deliveryOptionService;
    private readonly ICategoryService _categoryService;
    private readonly IFileService _fileService;
    private readonly IAdvancedService<ProductReview> _reviewService;
    private readonly IMapper _mapper;

    public AdminService(IProductService productService,
        IAdvancedService<DeliveryOption> deliveryOptionService,
        IFileService fileService,
        ICategoryService categoryService,
        IAdvancedService<ProductReview> reviewService,
        IMapper mapper)
    {
        _productService = productService;
        _deliveryOptionService = deliveryOptionService;
        _fileService = fileService;
        _categoryService = categoryService;
        _reviewService = reviewService;
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
        product.Entity.Status = isApproved ? ProductStatus.Active : ProductStatus.Awaiting;
        
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
        ServiceResponse serviceResponse = new ServiceResponse();
        
        var review = await _reviewService.GetAsync(reviewId);

        if (!review.IsSuccess)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = review.Message;
            
            return serviceResponse;
        }
        
        review.Entity.IsReviewed = true;
        review.Entity.IsApproved = isApproved;
        
        var updateRes = await _reviewService.UpdateAsync(review.Entity);
        
        if (!updateRes.IsSuccess)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = updateRes.Message;
            
            return serviceResponse;
        }
        
        serviceResponse.IsSuccess = true;
        return serviceResponse;
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
    

    public async Task<ServiceResponse> CreateCategoryAsync(CreateCategory createCategory)
    {
        var response = new ServiceResponse();

        if (createCategory == null || string.IsNullOrWhiteSpace(createCategory.Name))
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.ArgumentIsNull(nameof(createCategory), nameof(CreateCategory));
            return response;
        }

        var category = _mapper.Map<Category>(createCategory);

        var createRes = await _categoryService.CreateAsync(category);
        response.IsSuccess = createRes.IsSuccess;
        response.Message = createRes.Message;
        return response;
    }

    public async Task<ServiceResponse> UpdateCategoryAsync(UpdateCategory updateCategory)
    {
        var response = new ServiceResponse();
        
        var toUpdate = _mapper.Map<Category>(updateCategory);
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