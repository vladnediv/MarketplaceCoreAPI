using System.Security.Claims;
using AutoMapper;
using BLL.Model;
using BLL.Model.DTO.Product;
using BLL.Service.Interface;
using BLL.Service.Interface.BasicInterface;
using BLL.Service.Model;
using BLL.Service.Model.Constants;
using BLL.Service.Model.DTO.Cart;
using BLL.Service.Model.DTO.Category;
using DAL.Repository.DTO;
using DAL.Repository.Interface;
using Domain.Model.Cart;
using Domain.Model.Product;

namespace BLL.Service;

public class MarketplaceService : IMarketplaceService
{
    private readonly IProductService _productService;
    private readonly IGenericService<ProductQuestion> _productQuestionService;
    private readonly IGenericService<ProductReview> _productReviewService;
    private readonly IAdvancedService<Cart> _cartService;
    private readonly IGenericService<CartItem> _cartItemService;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public MarketplaceService(IProductService productService,
        IGenericService<ProductQuestion> productQuestionService,
        IGenericService<ProductReview> productReviewService,
        IAdvancedService<Cart>  cartService,
        IGenericService<CartItem> cartItemService,
        ICategoryService categoryService,
        IMapper mapper)
    {
        _productService = productService;
        _productQuestionService = productQuestionService;
        _productReviewService = productReviewService;
        _cartService = cartService;
        _cartItemService = cartItemService;
        _categoryService = categoryService;
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

    public async Task<ServiceResponse<MarketplaceProductView>> GetProductsByCategoryAsync(int categoryId)
    {
        throw new NotImplementedException();
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

    public async Task<ServiceResponse> UploadCartToUserAsync(List<CartItemDTO> cartItems, ClaimsPrincipal user)
    {
        var response = new ServiceResponse();
        
        //check if arguments are null
        if (!user.Claims.Any() || cartItems.Count == 0)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.ArgumentIsNull($"{nameof(user)} or {nameof(cartItems)}", $"{nameof(ClaimsPrincipal)} + {nameof(List<CartItemDTO>)}");
            
            return response;
        }
        
        //check if user id == 0
        var id = GetUserIdFromClaims(user);
        if (id == 0)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UserNotFound;
            
            return response;
        }
        
        
        //create cart
        //map from cartItemDTO to CartItem
        var mappedCartItems = cartItems.Select(x => _mapper.Map<CartItem>(x)).ToList();
        
        bool exists = false;
        //check if cart with the user id exists
        var getCartRes = await _cartService.FirstOrDefaultAsync(x => x.UserId == id);
        //if cart not found, create new
        if (!getCartRes.IsSuccess)
        {
            var createRes = await _cartService.CreateAsync(new Cart() { UserId = id, CartItems = mappedCartItems });

            if (!createRes.IsSuccess)
            {
                response.IsSuccess = false;
                response.Message = createRes.Message;
            
                return response;
            }
            
            response.IsSuccess = true;
        
            return response;
        }
        //if cart exists, do nothing
        response.IsSuccess = false;
        response.Message = ServiceResponseMessages.AlreadyExists("userCart", nameof(Cart));
        
        return response;
    }

    public async Task<ServiceResponse<CartDTO>> GetCartAsync(ClaimsPrincipal user)
    {
        var response =  new ServiceResponse<CartDTO>();
        
        //check if arguments are null
        if (!user.Claims.Any())
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.ArgumentIsNull(nameof(user), nameof(ClaimsPrincipal));
            
            return response;
        }
        
        //get userId from userClaims
        var userId = GetUserIdFromClaims(user);
        if (userId == 0)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UserNotFound;
            
            return response;
        }

        var cart = await _cartService.FirstOrDefaultAsync(x => x.UserId == userId);

        if (!cart.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = cart.Message;
            
            return response;
        }
        
        response.IsSuccess = true;
        response.Entity = _mapper.Map<CartDTO>(cart.Entity);
        
        return response;
    }

    public async Task<ServiceResponse> RemoveItemFromCartAsync(CartItemDTO cartItem, ClaimsPrincipal user)
    {
        var response = new ServiceResponse();

        //check if arguments are null
        if (cartItem.ProductId == 0 || !user.Claims.Any())
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.ArgumentIsNull(nameof(cartItem), nameof(List<CartItemDTO>));
            
            return response;
        }
        
        //get userId from userClaims
        var userId = GetUserIdFromClaims(user);
        if (userId == 0)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UserNotFound;
            
            return response;
        }
        
        //get CartItems from the Cart and check if the product in the argument already exists in the Cart
        var cart =  await _cartService.FirstOrDefaultAsync(x => x.UserId == userId);

        if (!cart.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = cart.Message;
            
            return response;
        }
        
        //if product in the CartItem is not present, then theres nothing to delete
        var buf = cart.Entity.CartItems.FirstOrDefault(x => x.ProductId == cartItem.ProductId);
        if (buf == null)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.EntityNotFound(nameof(CartItem));
            
            return response;
        }
        //if product exists in the Cart -> decrease quantity or delete if quantity == 1
        bool deleteCartItem = buf.Quantity == 1;

        if (deleteCartItem)
        {
            var deleteRes = await _cartItemService.DeleteAsync(buf);

            if (!deleteRes.IsSuccess)
            {
                response.IsSuccess = false;
                response.Message = deleteRes.Message;
                
                return response;
            }
            
            response.IsSuccess = true;
            
            return response;
        }

        buf.Quantity--;
        var updateRes = await _cartItemService.UpdateAsync(_mapper.Map<CartItem>(buf));
        if (!updateRes.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = updateRes.Message;
            
            return response;
        }
        
        response.IsSuccess = true;
        return response;
    }

    public async Task<ServiceResponse> AddItemToCartAsync(CartItemDTO cartItem, ClaimsPrincipal user)
    {
        var response = new ServiceResponse();

        //check if arguments are null
        if (cartItem.ProductId == 0 || !user.Claims.Any())
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.ArgumentIsNull(nameof(cartItem), nameof(List<CartItemDTO>));
            
            return response;
        }
        
        //get userId from userClaims
        var userId = GetUserIdFromClaims(user);
        if (userId == 0)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UserNotFound;
            
            return response;
        }
        
        //get CartItems from the Cart and check if the product in the argument already exists in the Cart
        var cart =  await _cartService.FirstOrDefaultAsync(x => x.UserId == userId);

        if (!cart.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = cart.Message;
            
            return response;
        }
        
        //if product in the CartItem is not present, create new CartItem
        var buf = cart.Entity.CartItems.FirstOrDefault(x => x.ProductId == cartItem.ProductId);
        if (buf == null)
        {
            var mappedCartItem = _mapper.Map<CartItem>(cartItem);
            mappedCartItem.Cart = cart.Entity;
            mappedCartItem.CartId = cart.Entity.Id;
            mappedCartItem.Quantity = 1;
            
            var createRes = await _cartItemService.CreateAsync(mappedCartItem);

            if (!createRes.IsSuccess)
            {
                response.IsSuccess = false;
                response.Message = createRes.Message;
                
                return response;
            }
            
            response.IsSuccess = true;
            
            return response;
        }
        //if product exists in the Cart -> increase quantity
        buf.Quantity++;
        var updateRes = await _cartItemService.UpdateAsync(_mapper.Map<CartItem>(buf));
        if (!updateRes.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = updateRes.Message;
            
            return response;
        }
        
        response.IsSuccess = true;
        return response;
    }

    public int GetUserIdFromClaims(ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0";
        return int.Parse(id);
    }

    public async Task<ServiceResponse<CategoryDTO>> GetSubcategoriesAsync(int parentCategoryId)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<CategoryDTO>> GetCategoryTreeAsync()
    {
        throw new NotImplementedException();
    }
}