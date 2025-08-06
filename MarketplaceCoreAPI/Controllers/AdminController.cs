using System.Threading.Tasks;
using BLL.Service.Interface;
using BLL.Service.Model;
using DAL.Repository.DTO;
using Domain.Model.Product;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceCoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : Controller
{
    private readonly IAdminService  _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    
    [HttpGet("GetAllProducts")]
    public async Task<IActionResult> GetAllProductsAsync()
    {
        ServiceResponse<AdminProductView> res = await _adminService.GetProductsByParameter(x => x.Id == x.Id);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpGet("CreateDeliveryOption")]
    public async Task<IActionResult> CreateDeliveryOptionAsync(string deliveryOption, decimal price)
    {
        ServiceResponse res = await _adminService.CreateDeliveryOptionAsync(deliveryOption, price);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpGet("GetDeliveryOptions")]
    public async Task<IActionResult> GetDeliveryOptionsAsync()
    {
        ServiceResponse<DeliveryOption> res = await _adminService.GetAllDeliveryOptionsAsync();
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpPost("EditProductApprovedStatus")]
    public async Task<IActionResult> EditProductApprovedStatusAsync(int productId, bool isApproved)
    {
        var res = await _adminService.EditProductApprovedStatusAsync(productId, isApproved);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
}