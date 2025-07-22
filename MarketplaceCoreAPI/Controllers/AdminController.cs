using System.Threading.Tasks;
using BLL.Service.Interface;
using BLL.Service.Model;
using Domain.Model.Product;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceCoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : Controller
{
    private readonly IAdminService  _service;

    public AdminController(IAdminService service)
    {
        _service = service;
    }
    
    [HttpGet("GetAllProducts")]
    public async Task<IActionResult> GetAllProductsAsync()
    {
        ServiceResponse<Product> res = await _service.GetProductsByParameter(x => x.Id == x.Id);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpGet("CreateDeliveryOption")]
    public async Task<IActionResult> CreateDeliveryOptionAsync(string deliveryOption)
    {
        ServiceResponse<DeliveryOption> res = await _service.CreateDeliveryOptionAsync(deliveryOption);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpGet("GetDeliveryOptions")]
    public async Task<IActionResult> GetDeliveryOptionsAsync()
    {
        ServiceResponse<DeliveryOption> res = await _service.GetAllDeliveryOptionsAsync();
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
}