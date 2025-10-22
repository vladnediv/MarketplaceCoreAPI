using System.Threading.Tasks;
using BLL.Model;
using BLL.Model.Constants;
using BLL.Model.DTO.Category;
using BLL.Model.DTO.Product;
using BLL.Service.Interface;
using Domain.Model.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceCoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{IdentityRoles.Admin},{IdentityRoles.SuperAdmin}")]
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
    [Authorize(Roles = IdentityRoles.SuperAdmin)]
    public async Task<IActionResult> CreateCategoryAsync(CreateCategory createCategory)
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
    [Authorize(Roles = IdentityRoles.SuperAdmin)]
    public async Task<IActionResult> DeleteCategoryAsync(int categoryId)
    {
        var res = await _adminService.DeleteCategoryAsync(categoryId);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
    [HttpPost("UpdateCategory")]
    [Authorize(Roles = IdentityRoles.SuperAdmin)]
    public async Task<IActionResult> UpdateCategoryAsync(UpdateCategory updateCategory)
    {
        var res = await _adminService.UpdateCategoryAsync(updateCategory);
        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }

    [HttpPost("EditReviewApprovedStatus")]
    public async Task<IActionResult> EditReviewApprovedStatusAsync(int reviewId, bool isApproved)
    {
        var res = await _adminService.EditProductReviewApprovedStatusAsync(reviewId, isApproved);

        if (res.IsSuccess)
        {
            return Ok(res);
        }
        return BadRequest(res);
    }
}