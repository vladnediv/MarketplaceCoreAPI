using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Service;
using BLL.Service.Model;
using Domain.Model.Product;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceCoreAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : Controller
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }
    
    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([FromBody]Product product)
    {
        ServiceResponse<Product> res = await _productService.CreateAsync(product);
        if (res.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(res);
    }

    [HttpGet("GetProducts")]
    public async Task<IActionResult> GetProducts()
    {
        ServiceResponse<Product> res = await _productService.GetAllAsync();
        if (res.IsSuccess)
        {
            return Ok(res);
        }

        return BadRequest(res);
    }
}