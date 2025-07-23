using System.Threading.Tasks;
using BLL.Service.Interface;
using BLL.Service.Model;
using DAL.Repository.DTO;
using Domain.Model.Product;
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

    [HttpPost("CreateReview")]
    public async Task<IActionResult> CreateReviewAsync(CreateProductReview entity)
    {
        ServiceResponse<CreateProductReview> res = await _marketplaceService.CreateProductReviewAsync(entity);

        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    
    [HttpPost("CreateQuestion")]
    public async Task<IActionResult> CreateQuestionAsync(CreateProductQuestion entity)
    {
        ServiceResponse<CreateProductQuestion> res = await _marketplaceService.CreateProductQuestionAsync(entity);

        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
}