using System.Threading.Tasks;
using BLL.Model;
using BLL.Model.DTO.Product;
using BLL.Service.Interface;
using BLL.Service.Model;
using BLL.Service.Model.DTO.Category;
using DAL.Repository.DTO;
using Domain.Model.Product;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceCoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("GetAllProducts")]
    public async Task<IActionResult> GetAllProductsAsync()
    {
        ServiceResponse<AdminProductView> res = await _adminService.GetProductsByParameterAsync(x => x.Id == x.Id);
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

    [HttpPost("CreateCategory")]
    public async Task<IActionResult> CreateCategoryAsync(CRUDCategory createCategory)
    {
        var res = await _adminService.CreateCategoryAsync(createCategory);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpGet("GetCategoryTree")]
    public async Task<IActionResult> GetCategoryTreeAsync()
    {
        var res = await _adminService.GetCategoryTreeAsync();

        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpDelete("DeleteCategory")]
    public async Task<IActionResult> DeleteCategoryAsync(int categoryId)
    {
        var res = await _adminService.DeleteCategoryAsync(categoryId);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

}