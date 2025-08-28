using System;
using System.Threading.Tasks;
using BLL.Model;
using BLL.Model.Constants;
using BLL.Model.DTO.Product;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestion;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestionAnswer;
using BLL.Model.DTO.Product.IncludedModels.ProductReview;
using BLL.Service.Interface;
using Domain.Model.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceCoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = IdentityRoles.Shop)]
public class ShopController : Controller
{
    private readonly IShopService _shopService;

    public ShopController(IShopService shopService)
    {
        _shopService = shopService;
    }

    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProductAsync([FromForm] CreateProduct product)
    {
        product.ProductBrandId = _shopService.GetUserIdFromClaims(User);
        if (product.ProductBrandId == 0)
        {
            return Unauthorized(new ServiceResponse() { IsSuccess = false, Message = ServiceResponseMessages.UserNotFound });
        }
        ServiceResponse res = await _shopService.CreateProductAsync(product);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    
    [HttpPost("UpdateProduct")]
    public async Task<IActionResult> UpdateProductAsync([FromForm] UpdateProduct updateProduct)
    {
        ServiceResponse res = await _shopService.UpdateProductAsync(updateProduct);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpDelete("DeleteProductById")]
    public async Task<IActionResult> DeleteProductAsync(int productId)
    {
        int UserId = _shopService.GetUserIdFromClaims(User);
        ServiceResponse res = await _shopService.DeleteProductByIdAsync(productId, UserId);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpGet("GetProductById")]
    public async Task<IActionResult> GetProductByIdAsync(int id)
    {
        ServiceResponse<ShopProductView> res = await _shopService.GetProductByIdAsync(id);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    
    [HttpGet("GetShopProducts")]
    public async Task<IActionResult> GetShopProductsAsync()
    {
        int UserId = _shopService.GetUserIdFromClaims(User);
        ServiceResponse<ShopProductView> res = await _shopService.GetProductsByParameterAsync(x => x.ProductBrandId == UserId);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    
    
    
    [HttpPost("AnswerQuestion")]
    public async Task<IActionResult> AnswerQuestionAsync([FromForm] CreateProductQuestionAnswer createProductQuestionAnswer)
    { 
        createProductQuestionAnswer.AuthorId = _shopService.GetUserIdFromClaims(User);
        ServiceResponse<CreateProductQuestionAnswer> res = await _shopService.CreateProductQuestionAnswerAsync(createProductQuestionAnswer);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpPost("GetShopQuestions")]
    public async Task<IActionResult> GetShopQuestionsAsync()
    {
        ServiceResponse res = new ServiceResponse();
        int shopId = _shopService.GetUserIdFromClaims(User);
        if (shopId == 0)
        {
            res.IsSuccess = false;
            res.Message = ServiceResponseMessages.UserNotFound;
            
            return Unauthorized(res);
        }
        
        var questions = await _shopService.GetProductQuestionsByParameterAsync(x => x.Product.ProductBrandId == shopId);
        if (questions.IsSuccess)
        {
            return Ok(questions);
        }
        return BadRequest(questions);
    }

    [HttpPost("GetQuestionsByParameter")]
    public async Task<IActionResult> GetQuestionsByParameterAsync(string parameter, string value)
    {
        ServiceResponse<ProductQuestionDTO> res = new ServiceResponse<ProductQuestionDTO>();
        res.IsSuccess = false;
        res.Message = ServiceResponseMessages.UnknownError;
        
        if (parameter == nameof(ProductQuestion.Question))
        {
            res = await _shopService.GetProductQuestionsByParameterAsync(x => x.Question == value);
        }

        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    [HttpGet("GetProductQuestions")]
    public async Task<IActionResult> GetProductQuestionsAsync(int productId)
    {
        ServiceResponse<ProductQuestionDTO> res = await _shopService.GetProductQuestionsByParameterAsync(x => x.ProductId == productId);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    
    

    [HttpGet("GetProductReviews")]
    public async Task<IActionResult> GetProductReviewsAsync(int productId)
    {
        ServiceResponse<ProductReviewDTO> res = await _shopService.GetProductReviewsByParameterAsync(x => x.ProductId == productId);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    [HttpPost("GetShopReviews")]
    public async Task<IActionResult> GetShopReviewsAsync()
    {
        ServiceResponse res = new ServiceResponse();
        int shopId = _shopService.GetUserIdFromClaims(User);
        if (shopId == 0)
        {
            res.IsSuccess = false;
            res.Message = ServiceResponseMessages.UserNotFound;
            
            return Unauthorized(res);
        }
        
        var reviews = await _shopService.GetProductReviewsByParameterAsync(x => x.Product.ProductBrandId == shopId);
        if (reviews.IsSuccess)
        {
            return Ok(reviews);
        }
        return BadRequest(reviews);
    }

    [HttpPost("GetReviewsByParameter")]
    public async Task<IActionResult> GetReviewsByParameterAsync(string parameter, string value)
    {
        ServiceResponse<ProductReviewDTO> res = new ServiceResponse<ProductReviewDTO>();
        res.IsSuccess = false;
        res.Message = ServiceResponseMessages.UnknownError;
        
        if (parameter == nameof(ProductReview.Rating))
        {
            try
            {
                var insert = int.Parse(value);
                res = await _shopService.GetProductReviewsByParameterAsync(x => x.Rating == insert);
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }
        }

        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    
    

    [HttpPost("EditProductActiveStatus")]
    public async Task<IActionResult> ActivateProductAsync(int productId, bool isActive)
    {
        int userId = _shopService.GetUserIdFromClaims(User);
        var res = await _shopService.EditProductActiveStatusAsync(productId, userId, isActive);

        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    

    [HttpGet("GetCategoryTree")]
    public async Task<IActionResult> GetCategoryTreeAsync()
    {
        var res = await _shopService.GetCategoryTreeAsync();
        if(res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpGet("GetSubcategoriesById")]
    public async Task<IActionResult> GetSubcategoriesAsync(int parentCategoryId)
    {
        var res = await _shopService.GetSubcategoriesAsync(parentCategoryId);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    
}