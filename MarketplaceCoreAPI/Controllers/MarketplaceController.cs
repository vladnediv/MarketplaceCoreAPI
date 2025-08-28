using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Model;
using BLL.Model.Constants;
using BLL.Model.DTO.Cart;
using BLL.Model.DTO.Order;
using BLL.Model.DTO.Product;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestion;
using BLL.Model.DTO.Product.IncludedModels.ProductReview;
using BLL.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceCoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarketplaceController : Controller
{
    private readonly IMarketplaceService  _marketplaceService;

    public MarketplaceController(IMarketplaceService marketplaceService)
    {
        _marketplaceService = marketplaceService;
    }

    [HttpGet("GetAllProducts")]
    public async Task<IActionResult> GetAllProductsAsync()
    {
        ServiceResponse<MarketplaceProductView> res = await _marketplaceService.GetProductsAsync();
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        else
        {
            return BadRequest(res);
        }
    }
    
    [HttpGet("SearchProductsByName")]
    public async Task<IActionResult> SearchProductsByNameAsync(string searchQuery)
    {
        ServiceResponse<ProductCardView> res = await _marketplaceService.GetProductsDTOAsync(searchQuery);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    
    [HttpGet("GetProductById")]
    public async Task<IActionResult> GetProductByIdAsync(int id)
    {
        ServiceResponse<MarketplaceProductView> res = await _marketplaceService.GetProductByIdAsync(id);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [Authorize(Roles = IdentityRoles.User)]
    [HttpPost("SaveCartToUser")]
    public async Task<IActionResult> SaveCartToUser(List<CartItemDTO> cartItems)
    {
        var res = await _marketplaceService.UploadCartToUserAsync(cartItems, User);

        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [Authorize(Roles = IdentityRoles.User)]
    [HttpPost("GetCart")]
    public async Task<IActionResult> GetCartAsync()
    {
        var res = await _marketplaceService.GetCartAsync(User);

        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    
    [Authorize(Roles = IdentityRoles.User)]
    [HttpPost("AddProductToCart")]
    public async Task<IActionResult> AddProductToCartAsync(CartItemDTO cartItem)
    {
        var res = await _marketplaceService.AddItemToCartAsync(cartItem, User);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [Authorize(Roles = IdentityRoles.User)]
    [HttpPost("RemoveProductFromCart")]
    public async Task<IActionResult> RemoveProductFromCartAsync(CartItemDTO cartItem)
    {
        var res = await _marketplaceService.RemoveItemFromCartAsync(cartItem, User);

        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    
    [Authorize(Roles = IdentityRoles.User)]
    [HttpPost("CreateReview")]
    public async Task<IActionResult> CreateReviewAsync([FromForm] CreateProductReview entity)
    {
        entity.UserId = _marketplaceService.GetUserIdFromClaims(User);
        //check if the userId is 0
        if (entity.UserId == 0)
        {
            return Unauthorized(new ServiceResponse() {IsSuccess = false, Message = ServiceResponseMessages.UserNotFound});
        }
        ServiceResponse<CreateProductReview> res = await _marketplaceService.CreateProductReviewAsync(entity);

        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    
    [Authorize(Roles = IdentityRoles.User)]
    [HttpPost("CreateQuestion")]
    public async Task<IActionResult> CreateQuestionAsync([FromForm]CreateProductQuestion entity)
    {
        entity.UserId = _marketplaceService.GetUserIdFromClaims(User);
        //check if the userId is 0
        if (entity.UserId == 0)
        {
            return Unauthorized(new ServiceResponse() {IsSuccess = false, Message = ServiceResponseMessages.UserNotFound});
        }
        ServiceResponse<CreateProductQuestion> res = await _marketplaceService.CreateProductQuestionAsync(entity);

        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
        
    }

    [HttpGet("GetCategoryTree")]
    public async Task<IActionResult> GetCategoryTreeAsync()
    {
        var res = await _marketplaceService.GetCategoryTreeAsync();
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpGet("GetRootCategories")]
    public async Task<IActionResult> GetRootCategoriesAsync()
    {
        var res = await _marketplaceService.GetRootCategoriesAsync();
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpGet("GetSubcategories")]
    public async Task<IActionResult> GetSubcategoriesAsync(int parentCategoryId)
    {
        var res = await _marketplaceService.GetSubcategoriesAsync(parentCategoryId);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpPost("CreateOrder")]
    [Authorize(Roles = IdentityRoles.User)]
    public async Task<IActionResult> CreateOrderAsync(CreateOrder entity)
    {
        entity.UserId = _marketplaceService.GetUserIdFromClaims(User);
        
        var res = _marketplaceService.CreateOrderAsync(entity);
        return Ok(res);
    }
}