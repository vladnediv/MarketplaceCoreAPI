using System.Security.Claims;
using AutoMapper;
using BLL.Model.DTO.Order;
using BLL.Service.Interface;
using BLL.Service.Model;
using BLL.Service.Model.Constants;
using BLL.Service.Model.DTO.Order;
using DAL.Repository.DTO;
using DAL.Repository.Interface;
using Domain.Model.Order;
using Domain.Model.Product;

namespace BLL.Service;

public class MarketplaceService : IMarketplaceService
{
    private readonly IProductService _productService;
    private readonly IGenericService<ProductQuestion> _productQuestionService;
    private readonly IGenericService<ProductReview> _productReviewService;
    private readonly IFileService _fileService;
    private readonly IAdvancedService<Order> _orderService;
    private readonly IMapper _mapper;

    public MarketplaceService(IProductService productService,
        IGenericService<ProductQuestion> productQuestionService,
        IGenericService<ProductReview> productReviewService,
        IFileService fileService,
        IAdvancedService<Order> orderService,
        IMapper mapper)
    {
        _productService = productService;
        _productQuestionService = productQuestionService;
        _productReviewService = productReviewService;
        _fileService = fileService;
        _orderService = orderService;
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
                
                //load the pictures of the product
                int i = 0;
                foreach (var mediaFile in productResponse.Entity.MediaFiles)
                {
                    if (mediaFile.MediaType == MediaType.Image)
                    {
                        //load the picture by the path
                        var loadRes = await _fileService.GetPictureAsync(mediaFile.Url);
                        if (loadRes.IsSuccess)
                        {
                            //assign the media content
                            apiResponse.Entity.MediaFiles[i].MediaContent = loadRes.Entity;
                        }
                    }
                    i++;
                }
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
        
        if (response.IsSuccess)
        {
            //iterate through each productCard
            foreach (var productCard in response.Entities)
            {
                //load the picture by the path
                var loadRes = await _fileService.GetPictureAsync(productCard.PictureUrl);
                if (loadRes.IsSuccess)
                {
                    //assign the media content
                    productCard.MediaContent = loadRes.Entity;
                }
            }
        }
        
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

    public async Task<ServiceResponse<OrderDTO>> CreateOrderAsync(CreateOrder entity)
    {
        var response = new ServiceResponse<OrderDTO>();
        
        //check if the products in the order are on stock
        foreach (var product in entity.OrderItems)
        {
            var res = await _productService.ModifyProductStockAsync(true, product.ProductId, product.Quantity);
            if (!res.IsSuccess)
            {
                response.IsSuccess = false;
                response.Message = res.Message;
                
                return response;
            }
        }
        
        //map the CreateOrder model to Order
        var order = _mapper.Map<Order>(entity);
        
        var createRes = await _orderService.CreateAsync(order);
        if (!createRes.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = createRes.Message;
            
            return response;
        }
        
        
        
        //TODO send orderCreatedEvent to some broker like RabbitMQ and subscribe on AuthAPI
        
        //temporary code
        //if order has been created, return the created Order
        var orderDTO = _mapper.Map<OrderDTO>(order);
        response.IsSuccess = true;
        response.Entity = orderDTO;
        
        return response;
    }

    public int GetUserIdFromClaims(ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0";
        return int.Parse(id);
    }
}