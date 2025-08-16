using System.Net;
using System.Threading.Tasks;
using BLL.Service.Interface;
using BLL.Service.Model;
using BLL.Service.Model.Constants;
using BLL.Service.Model.DTO.Order;
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
    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrderAsync(CreateOrder entity)
    {
        //get the user id from the access token
        entity.UserId = _marketplaceService.GetUserIdFromClaims(User);

        //if user id == 0 return unauthorized
        if (entity.UserId == 0)
        {
            return Unauthorized(new ServiceResponse() {IsSuccess = false, Message = ServiceResponseMessages.UserNotFound});
        }
        
        
        //create the order
        var res = await _marketplaceService.CreateOrderAsync(entity);

        //if could not create order, return BadRequest
        if (!res.IsSuccess)
        {
            return BadRequest(res);
        }
        
        //if order created, get the order entity and return to the client
        return Ok(res);
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