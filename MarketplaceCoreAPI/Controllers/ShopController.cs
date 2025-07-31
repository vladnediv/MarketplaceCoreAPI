using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Service.Interface;
using BLL.Service.Model;
using BLL.Service.Model.Constants;
using DAL.Repository.DTO;
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
    public async Task<IActionResult> CreateProductAsync(CreateProduct product)
    {
        //product.ProductBrandId = UserId;
        product.ProductBrandId = _shopService.GetUserIdFromClaims(User);
        ServiceResponse res = await _shopService.CreateProductAsync(product);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    //TODO Fix this method
    [HttpPost("UpdateProduct")]
    public async Task<IActionResult> UpdateProductAsync(UpdateProduct updateProduct)
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
    public async Task<IActionResult> AnswerQuestionAsync(CreateProductQuestionAnswer createProductQuestionAnswer)
    { 
        createProductQuestionAnswer.AuthorId = _shopService.GetUserIdFromClaims(User);
        ServiceResponse<CreateProductQuestionAnswer> res = await _shopService.CreateProductQuestionAnswerAsync(createProductQuestionAnswer);
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
    
}