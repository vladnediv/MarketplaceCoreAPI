using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using BLL.Model;
using BLL.Model.Constants;
using BLL.Model.DTO.Category;
using BLL.Model.DTO.Product;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestion;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestionAnswer;
using BLL.Model.DTO.Product.IncludedModels.ProductReview;
using BLL.Service.Interface;
using BLL.Service.Interface.BasicInterface;
using Domain.Model.Product;

namespace BLL.Service;

public class ShopService : IShopService
{
    private readonly IProductService _productService;
    private readonly IGenericService<ProductQuestionAnswer> _questionAnswerService;
    //private readonly IAdvancedService<Order> _orderService;
    private readonly IAdvancedService<ProductReview> _reviewService;
    private readonly IAdvancedService<ProductQuestion> _questionService;
    private readonly IAdvancedService<DeliveryOption> _deliveryOptionService;
    private readonly IFileService _fileService;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public ShopService(
        IProductService productService,
        IGenericService<ProductQuestionAnswer> questionAnswerService,
        IAdvancedService<ProductReview> reviewService,
        IAdvancedService<ProductQuestion> questionService,
        IAdvancedService<DeliveryOption> deliveryOptionService,
        IFileService fileService,
        ICategoryService categoryService,
        IMapper mapper)
    {
        _productService = productService;
        _questionAnswerService = questionAnswerService;
        _reviewService = reviewService;
        _questionService = questionService;
        _deliveryOptionService = deliveryOptionService;
        _fileService = fileService;
        _categoryService = categoryService;
        _mapper = mapper;
    }

    public async Task<ServiceResponse> CreateProductAsync(CreateProduct product)
    {
        //map createProduct to Product
        Product entity = _mapper.Map<CreateProduct, Product>(product);

        int i = 0;
        //save every picture in the mediaFiles
        foreach (var media in product.MediaFiles)
        {
            if (media.MediaType == MediaType.Image)
            {
                //try save the picture
                var saveRes = await _fileService.SavePictureAsync(media.File);
                //if save completes, save the path to the picture into the Product model
                if (saveRes.IsSuccess)
                {
                    entity.MediaFiles.ElementAt(i).Url = saveRes.Entity;
                }
            }

            i++;
        }

        //after pictures are saved and the path urls are passed into the product entity, create Product
        ServiceResponse<Product> response = await _productService.CreateAsync(entity);
        
        ServiceResponse serviceResponse = new ServiceResponse();
        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
        }
        else
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = response.Message;
        }
        
        return serviceResponse;
    }

    public async Task<ServiceResponse> UpdateProductAsync(UpdateProduct updateProduct)
    {
        Product entity = _mapper.Map<Product>(updateProduct);
        
        
        var oldProduct = await _productService.GetAsync(entity.Id);
        
        //TODO error with updating delivery options
        
        if (oldProduct.IsSuccess)
        {
            oldProduct.Entity.Name = entity.Name;
            oldProduct.Entity.Price = entity.Price;
            oldProduct.Entity.Stock = entity.Stock;
            oldProduct.Entity.DiscountValue = entity.DiscountValue;
            oldProduct.Entity.MediaFiles = entity.MediaFiles;
            oldProduct.Entity.Characteristics = entity.Characteristics;
        }
        else
        {
            return new ServiceResponse()
            {
                IsSuccess = false,
                Message = oldProduct.Message
            };
        }
        
        ServiceResponse<Product> response = await _productService.UpdateAsync(oldProduct.Entity);
        
        ServiceResponse serviceResponse = new ServiceResponse();
        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
        }
        else
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = response.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse> DeleteProductByIdAsync(int id, int userId)
    {
        //check if the product exists
        var res = await _productService.FirstOrDefaultAsync(x => x.ProductBrandId == userId && x.Id == id);
        if (!res.IsSuccess)
        {
            return new ServiceResponse()
            {
                IsSuccess = false,
                Message = res.Message
            };
        }
        //delete the product by ID
        ServiceResponse<Product> response = await _productService.DeleteByIdAsync(id);
        
        ServiceResponse serviceResponse = new ServiceResponse();
        if (response.IsSuccess)
        {
            //delete the media files of type "Image" from the server
            foreach (var mediaFile in res.Entity.MediaFiles)
            {
                if (mediaFile.MediaType == MediaType.Image)
                { 
                    await _fileService.DeletePictureAsync(mediaFile.Url);
                }
            }
            
            serviceResponse.IsSuccess = true;
        }
        else
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = response.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<ShopProductView>> GetProductByIdAsync(int id)
    {
        //get the product by ID
        ServiceResponse<Product> res = await _productService.GetAsync(id);
        ServiceResponse<ShopProductView> response = new ServiceResponse<ShopProductView>();
        if (res.IsSuccess)
        {
            //map the product to ShopProductView
            ShopProductView entity = _mapper.Map<Product, ShopProductView>(res.Entity);
            response.Entity = entity;
            response.IsSuccess = true;
        }
        else
        {
            response.IsSuccess = false;
            response.Message = res.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ShopProductView>> GetProductsByParameterAsync(Expression<Func<Product, bool>> predicate)
    {
        //get the products by parameter
        ServiceResponse<Product> response = await _productService.GetAllAsync(predicate);
        ServiceResponse<ShopProductView> res = new ServiceResponse<ShopProductView>();
        if (response.IsSuccess)
        {
            //map the products to List<ShopProductView>
            List<ShopProductView> entities = response.Entities.Select(x => _mapper.Map<Product, ShopProductView>(x)).ToList();
            
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

    public async Task<ServiceResponse<CreateProductQuestionAnswer>> CreateProductQuestionAnswerAsync(CreateProductQuestionAnswer createProductQuestionAnswer)
    {
        ProductQuestionAnswer entity = _mapper.Map<ProductQuestionAnswer>(createProductQuestionAnswer);
        ServiceResponse<ProductQuestionAnswer> response = await _questionAnswerService.CreateAsync(entity);
        
        ServiceResponse<CreateProductQuestionAnswer> serviceResponse = new ServiceResponse<CreateProductQuestionAnswer>();

        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
        }
        else
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = response.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<ProductReviewDTO>> GetProductReviewsByParameterAsync(Expression<Func<ProductReview, bool>> predicate)
    {
        ServiceResponse<ProductReview> response = await _reviewService.GetAllAsync(predicate);
        
        ServiceResponse<ProductReviewDTO> serviceResponse = new ServiceResponse<ProductReviewDTO>();
        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
            serviceResponse.Entities = response.Entities.Select(x => _mapper.Map<ProductReview, ProductReviewDTO>(x)).ToList();
        }
        else
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = response.Message;
        }
        
        return serviceResponse;
    }

    public async Task<ServiceResponse<ProductQuestionDTO>> GetProductQuestionsByParameterAsync(Expression<Func<ProductQuestion, bool>> predicate)
    {
        ServiceResponse<ProductQuestion> response = await _questionService.GetAllAsync(predicate);
        
        ServiceResponse<ProductQuestionDTO> serviceResponse = new ServiceResponse<ProductQuestionDTO>();
        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
            serviceResponse.Entities = response.Entities.Select(x => _mapper.Map<ProductQuestion, ProductQuestionDTO>(x)).ToList();
        }
        else
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = response.Message;
        }

        return serviceResponse;
    }
    public async Task<ServiceResponse> EditProductActiveStatusAsync(int productId, int userId, bool isActive)
    {
        ServiceResponse serviceResponse = new ServiceResponse();
        
        var product = await _productService.GetAsync(productId);

        if (!product.IsSuccess)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = product.Message;
            
            return serviceResponse;
        }

        if (product.Entity.ProductBrandId != userId)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = ServiceResponseMessages.AccessDenied(nameof(Product), userId);
            
            return serviceResponse;
        }
        
        product.Entity.IsActive = isActive;
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

    public async Task<ServiceResponse<CategoryDTO>> GetSubcategoriesAsync(int parentCategoryId)
    {
        var response = new ServiceResponse<CategoryDTO>();

        var res = await _categoryService.GetSubcategoriesByParentIdAsync(parentCategoryId);
        if (res.IsSuccess)
        {
            response.IsSuccess = true;
            response.Entities = res.Entities.Select(c => _mapper.Map<CategoryDTO>(c)).ToList();
        }
        else
        {
            response.IsSuccess = false;
            response.Message = res.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<CategoryDTO>> GetCategoryTreeAsync()
    {
        var response = new ServiceResponse<CategoryDTO>();
        var res = await _categoryService.GetCategoryTreeAsync();
        if (res.IsSuccess)
        {
            response.IsSuccess = true;
            response.Entities = res.Entities.Select(c => _mapper.Map<CategoryDTO>(c)).ToList();
        }
        else
        {
            response.IsSuccess = false;
            response.Message = res.Message;
        }
        return response;
    }

    // public async Task<ServiceResponse<Order>> GetOrdersByParameterAsync(Expression<Func<Order, bool>> predicate)
    // {
    //     ServiceResponse<Order> response = new ServiceResponse<Order>();
    //     
    //     //TODO use OrderService
    //     
    //     response.IsSuccess = false;
    //     return response;
    // }
    
    public int GetUserIdFromClaims(ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0";
        return int.Parse(id);
    }
}