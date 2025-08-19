using System.Security.Claims;
using AutoMapper;
using BLL.Service.Interface;
using BLL.Service.Model;
using BLL.Service.Model.Constants;
using DAL.Repository.DTO;
using DAL.Repository.Interface;
using Domain.Model.Product;

namespace BLL.Service;

public class MarketplaceService : IMarketplaceService
{
    private readonly IProductService _productService;
    private readonly IGenericService<ProductQuestion> _productQuestionService;
    private readonly IGenericService<ProductReview> _productReviewService;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public MarketplaceService(IProductService productService,
        IGenericService<ProductQuestion> productQuestionService,
        IGenericService<ProductReview> productReviewService,
        IFileService fileService,
        IMapper mapper)
    {
        _productService = productService;
        _productQuestionService = productQuestionService;
        _productReviewService = productReviewService;
        _fileService = fileService;
        _mapper = mapper;
    }
    
        
    public async Task<ServiceResponse<MarketplaceProductView>> GetProductByIdAsync(int id)
    {
        //get the product by ID
        ServiceResponse<Product> productResponse = await _productService.GetAsync(id);
        ServiceResponse<MarketplaceProductView> apiResponse = new ServiceResponse<MarketplaceProductView>();
        if (productResponse.IsSuccess)
        {
            //check if the product can be viewed to the user
            if (!productResponse.Entity.IsActive
                || !productResponse.Entity.IsApproved
                || !productResponse.Entity.IsReviewed)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Message = ServiceResponseMessages.ProductDeactivated(productResponse.Entity.Name);
            }
            else
            {
                //map the product to MarketplaceProductView
                apiResponse.IsSuccess = true; 
                apiResponse.Entity = _mapper.Map<MarketplaceProductView>(productResponse.Entity);
            }
        }
        else
        {
            apiResponse.IsSuccess = false;
            apiResponse.Message = productResponse.Message;
        }
        
        return apiResponse;
    }

    public async Task<ServiceResponse<ProductCardView>> GetProductsDTOAsync(string searchQuery)
    {
        //get the product cards by parameters
        ServiceResponse<ProductCardView> response = await _productService.GetProductCards
        (searchQuery, 
            x => x.IsActive && x.IsApproved && x.IsReviewed);
        
        return response;
    }

    public async Task<ServiceResponse<MarketplaceProductView>> GetProductsAsync()
    {
        ServiceResponse<Product> products = await _productService.GetAllAsync(x => x.IsActive &&
            x.IsApproved && x.IsReviewed);
        
        ServiceResponse<MarketplaceProductView> apiResponse = new ServiceResponse<MarketplaceProductView>();
        if (products.IsSuccess)
        {
            apiResponse.IsSuccess = true;
            apiResponse.Entities = products.Entities.Select(x => _mapper.Map<MarketplaceProductView>(x)).ToList();
        }
        else
        {
            apiResponse.IsSuccess = false;
            apiResponse.Message = products.Message;
        }
        return apiResponse;
    }

    public async Task<ServiceResponse<CreateProductQuestion>> CreateProductQuestionAsync(CreateProductQuestion entity)
    {
        ProductQuestion productQuestion = _mapper.Map<ProductQuestion>(entity);
        ServiceResponse<ProductQuestion> serviceResponse = await _productQuestionService.CreateAsync(productQuestion);
        ServiceResponse<CreateProductQuestion> apiResponse = new ServiceResponse<CreateProductQuestion>();
        if (serviceResponse.IsSuccess)
        {
            apiResponse.IsSuccess = true;
        }
        else
        {
            apiResponse.IsSuccess = false;
            apiResponse.Message = serviceResponse.Message;
        }
        //TODO Notify shop about new question
        return apiResponse;
    }

    public async Task<ServiceResponse<CreateProductReview>> CreateProductReviewAsync(CreateProductReview entity)
    {
        ProductReview productReview = _mapper.Map<ProductReview>(entity);
        ServiceResponse<ProductReview> response = await _productReviewService.CreateAsync(productReview);
        ServiceResponse<CreateProductReview> apiResponse = new ServiceResponse<CreateProductReview>();
        if (response.IsSuccess)
        {
            apiResponse.IsSuccess = true;
        }
        else
        {
            apiResponse.IsSuccess = false;
            apiResponse.Message = response.Message;
        }
        //TODO Notify shop about new review
        return apiResponse;
    }

    public int GetUserIdFromClaims(ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0";
        return int.Parse(id);
    }
}