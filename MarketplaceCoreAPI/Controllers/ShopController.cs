using System.Threading.Tasks;
using BLL.Service.Interface;
using BLL.Service.Model;
using Domain.Model.Product;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceCoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopController : Controller
{
    private readonly IShopService _shopService;

    public ShopController(IShopService shopService)
    {
        _shopService = shopService;
    }
    
    //TODO Create DTO for Product
    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProductAsync(Product product)
    {
        ServiceResponse<Product> res = await _shopService.CreateProductAsync(product);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpPost("UpdateProduct")]
    public async Task<IActionResult> UpdateProductAsync(Product product)
    {
        ServiceResponse<Product> res = await _shopService.UpdateProductAsync(product);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpDelete("DeleteProductById")]
    public async Task<IActionResult> DeleteProductAsync(int productId)
    {
        ServiceResponse<Product> res = await _shopService.DeleteProductByIdAsync(productId);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpGet("GetProductById")]
    public async Task<IActionResult> GetProductByIdAsync(int id)
    {
        ServiceResponse<Product> res = await _shopService.GetProductByIdAsync(id);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpGet("GetShopProducts")]
    public async Task<IActionResult> GetShopProductsAsync(int shopId)
    {
        ServiceResponse<Product> res = await _shopService.GetProductsByParameterAsync(x => x.ProductBrandId == shopId);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    //TODO Create DTO for ProductQuestionAnswer
    [HttpPost("AnswerQuestion")]
    public async Task<IActionResult> AnswerQuestionAsync(ProductQuestionAnswer productQuestionAnswer)
    {
        ServiceResponse<ProductQuestionAnswer> res = await _shopService.CreateProductQuestionAnswerAsync(productQuestionAnswer);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpGet("GetProductReviews")]
    public async Task<IActionResult> GetProductReviewsAsync(int productId)
    {
        ServiceResponse<ProductReview> res = await _shopService.GetProductReviewsByParameterAsync(x => x.ProductId == productId);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpGet("GetProductQuestions")]
    public async Task<IActionResult> GetProductQuestionsAsync(int productId)
    {
        ServiceResponse<ProductQuestion> res = await _shopService.GetProductQuestionsByParameterAsync(x => x.ProductId == productId);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
}