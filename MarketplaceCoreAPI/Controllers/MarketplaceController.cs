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
    
    [HttpGet("SearchProductsByName")]
    public async Task<IActionResult> SearchProductsByNameAsync(string searchQuery)
    {
        ServiceResponse<ProductCardDTO> res = await _marketplaceService.GetProductsDTOAsync(searchQuery);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    
    [HttpGet("GetProductById")]
    public async Task<IActionResult> GetProductByIdAsync(int id)
    {
        ServiceResponse<Product> res = await _marketplaceService.GetProductByIdAsync(id);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpPost("CreateReview")]
    public async Task<IActionResult> CreateReviewAsync(ProductReview entity)
    {
        ServiceResponse<ProductReview> res = await _marketplaceService.CreateProductReviewAsync(entity);

        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    
    [HttpPost("CreateQuestion")]
    public async Task<IActionResult> CreateQuestionAsync(ProductQuestion entity)
    {
        ServiceResponse<ProductQuestion> res = await _marketplaceService.CreateProductQuestionAsync(entity);

        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
}