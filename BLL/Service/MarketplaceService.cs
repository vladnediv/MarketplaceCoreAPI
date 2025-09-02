using System.Security.Claims;
using AutoMapper;
using BLL.Model;
using BLL.Model.Constants;
using BLL.Model.DTO.Cart;
using BLL.Model.DTO.Category;
using BLL.Model.DTO.Order;
using BLL.Model.DTO.Product;
using BLL.Model.DTO.Product.IncludedModels;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestion;
using BLL.Model.DTO.Product.IncludedModels.ProductReview;
using BLL.Service.Interface;
using BLL.Service.Interface.BasicInterface;
using Domain.Model.Cart;
using Domain.Model.Order;
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
    private readonly IAdvancedService<Order> _orderService;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public MarketplaceService(IProductService productService,
        IGenericService<ProductQuestion> productQuestionService,
        IGenericService<ProductReview> productReviewService,
        IAdvancedService<Cart>  cartService,
        IGenericService<CartItem> cartItemService,
        ICategoryService categoryService,
        IAdvancedService<Order> orderService,
        IFileService fileService,
        IMapper mapper)
    {
        _productService = productService;
        _productQuestionService = productQuestionService;
        _productReviewService = productReviewService;
        _cartService = cartService;
        _cartItemService = cartItemService;
        _categoryService = categoryService;
        _orderService = orderService;
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
                //sort out not approved and not reviewed reviews
                productResponse.Entity.Reviews = productResponse.Entity.Reviews.Where(x => x.IsApproved && x.IsReviewed);
                
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

            foreach (var product in products.Entities)
            {
                product.Reviews = product.Reviews.Where(x => x.IsApproved && x.IsReviewed).ToList();
            }
            
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
        var response = new ServiceResponse<MarketplaceProductView>();

        // Validate category existence
        var categoryRes = await _categoryService.GetAsync(categoryId);
        if (!categoryRes.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = categoryRes.Message;
            return response;
        }

        // Fetch products by category and public visibility constraints
        var productsRes = await _productService.GetAllAsync(p =>
            p.CategoryId == categoryId && p.IsActive && p.IsApproved && p.IsReviewed);

        if (!productsRes.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = productsRes.Message;
            return response;
        }

        response.IsSuccess = true;
        var buf = productsRes.Entities.Select(x => x.Reviews.Where(x => x.IsApproved && x.IsReviewed)).ToList();
        response.Entities = buf.Select(x => _mapper.Map<MarketplaceProductView>(x)).ToList();
        return response;
    }
    
    
    

    public async Task<ServiceResponse<CreateProductQuestion>> CreateProductQuestionAsync(CreateProductQuestion entity)
    {
        ProductQuestion productQuestion = _mapper.Map<ProductQuestion>(entity);
        productQuestion.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
        
        //save pictures from the question

        if (productQuestion.MediaFiles != null && productQuestion.MediaFiles.Any())
        {
            int i = 0;
            foreach (var media in entity.MediaFiles)
            {
                if (media.MediaType == MediaType.Image)
                {
                    var url = await _fileService.SavePictureAsync(media.File);
                    if (url.IsSuccess)
                    {
                        productQuestion.MediaFiles.ElementAt(i).Url = url.Entity;
                    }
                }

                i++;
            }
        }
        
        //create the question
        ServiceResponse<ProductQuestion> serviceResponse = await _productQuestionService.CreateAsync(productQuestion);
        ServiceResponse<CreateProductQuestion> apiResponse = new ServiceResponse<CreateProductQuestion>();
        if (serviceResponse.IsSuccess)
        {
            apiResponse.IsSuccess = true;
        }
        else
        {
            //delete the pictures, because the creation of ProductQuestion failed
            foreach (var media in entity.MediaFiles)
            {
                if (media.MediaType == MediaType.Image)
                {
                    await _fileService.DeletePictureAsync(media.Url);
                }
            }
            
            apiResponse.IsSuccess = false;
            apiResponse.Message = serviceResponse.Message;
        }
        //TODO Notify shop about new question
        return apiResponse;
    }
    
    public async Task<ServiceResponse<CreateProductReview>> CreateProductReviewAsync(CreateProductReview entity)
    {
        ProductReview productReview = _mapper.Map<ProductReview>(entity);
        
        //save the pictures from review (if there are any)
        int i = 0;

        if (entity.MediaFiles != null  && entity.MediaFiles.Any())
        {
            foreach (var media in entity.MediaFiles)
            {
                if (media.MediaType == MediaType.Image)
                {
                    var url = await _fileService.SavePictureAsync(media.File);
                    if (url.IsSuccess)
                    {
                        entity.MediaFiles.ElementAt(i).Url = url.Entity;
                    }
                }

                i++;
            }
        }
        
        ServiceResponse<ProductReview> response = await _productReviewService.CreateAsync(productReview);
        ServiceResponse<CreateProductReview> apiResponse = new ServiceResponse<CreateProductReview>();
        if (response.IsSuccess)
        {
            apiResponse.IsSuccess = true;
        }
        else
        {
            //delete pictures, because couldnt create review
            foreach (var media in entity.MediaFiles)
            {
                if (media.MediaType == MediaType.Image)
                {
                    await _fileService.DeletePictureAsync(media.Url);
                }
            }
            
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

    public async Task<ServiceResponse<CategoryDTO>> GetRootCategoriesAsync()
    {
        var response = new ServiceResponse<CategoryDTO>();
        
        var rootCategories = await _categoryService.GetRootCategoriesAsync();
        if (rootCategories.IsSuccess)
        {
            response.IsSuccess = true;
            response.Entities = rootCategories.Entities.Select(c => _mapper.Map<CategoryDTO>(c)).ToList();

            return response;
        }
        response.IsSuccess = false;
        response.Message = rootCategories.Message;
        
        return response;
    }
    
    
    
    
    public async Task<ServiceResponse<MarketplaceOrderView>> CreateOrderAsync(CreateOrder entity)
    {
        var response = new ServiceResponse<MarketplaceOrderView>();
        
        //check if the products in the order are on stock
        foreach (var product in entity.OrderItems)
        {
            var res = await _productService.CheckIfProductOnStock(product.ProductId, product.Quantity);
            if (!res.IsSuccess)
            {
                response.IsSuccess = false;
                response.Message = res.Message;
                
                return response;
            }
        }
        
        //if all products are on stock, update the db model values
        int i = 0;
        foreach (var product in entity.OrderItems)
        {
            var res = await _productService.ModifyProductStockAsync(true, product.ProductId, product.Quantity);
            if (!res.IsSuccess)
            {
                //if something went wrong while modifying stock value, restore the previous state
                for (int y = 0; y < i; y++)
                {
                    var orderItem = entity.OrderItems.ElementAt(y);
                    await _productService.ModifyProductStockAsync(false, orderItem.ProductId, orderItem.Quantity);
                }

                response.IsSuccess = false;
                response.Message = res.Message;
                
                return response;
            }

            i++;
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
        var orderDTO = _mapper.Map<MarketplaceOrderView>(order);
        response.IsSuccess = true;
        response.Entity = orderDTO;
        
        return response;
    }

    public async Task<ServiceResponse<MarketplaceOrderView>> GetOrderByIdAsync(int id, ClaimsPrincipal user)
    {
        var response = new ServiceResponse<MarketplaceOrderView>();
        
        var userId = GetUserIdFromClaims(user);
        if (userId == 0)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UserNotFound;
            
            return response;
        }
        
        //get order by id
        var orderRes = await _orderService.GetAsync(id);
        if (!orderRes.IsSuccess)
        {
            //if something went wrong, return result
            response.IsSuccess = false;
            response.Message = orderRes.Message;
            
            return response;
        }

        if (orderRes.Entity.UserId != userId)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.AccessDenied(nameof(Order), id);
            
            return response;
        }
        
        //map to MarketplaceOrderView from Order
        var marketplaceOrder = _mapper.Map<MarketplaceOrderView>(orderRes.Entity);
        
        response.IsSuccess = true;
        response.Entity = marketplaceOrder;
        
        return response;
    }

    public async Task<ServiceResponse<MarketplaceOrderView>> GetUserOrdersAsync(ClaimsPrincipal user)
    {
        var response = new ServiceResponse<MarketplaceOrderView>();
        
        //get user id from claims
        var userId = GetUserIdFromClaims(user);
        if (userId == 0)
        {
            //if could not get the id from claims, return response
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UserNotFound;
            
            return response;
        }
        
        //search for orders placed by user
        var orders = await _orderService.GetAllAsync(x => x.UserId == userId);

        if (!orders.IsSuccess)
        {
            //if something went wrong, return response
            response.IsSuccess = false;
            response.Message = orders.Message;
            
            return response;
        }
        
        response.IsSuccess = true;
        response.Entities = orders.Entities.Select(x => _mapper.Map<MarketplaceOrderView>(x)).ToList();
        
        return response;
    }
}