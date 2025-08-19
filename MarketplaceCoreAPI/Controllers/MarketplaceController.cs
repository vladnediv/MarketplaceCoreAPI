using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BLL.Service.Interface;
using BLL.Service.Model;
using BLL.Service.Model.Constants;
using BLL.Service.Model.DTO.Cart;
using DAL.Repository.DTO;
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
    public async Task<IActionResult> CreateReviewAsync(CreateProductReview entity)
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
    public async Task<IActionResult> CreateQuestionAsync(CreateProductQuestion entity)
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
}