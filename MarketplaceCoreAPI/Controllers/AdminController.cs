using System.Threading.Tasks;
using BLL.Service.Interface;
using BLL.Service.Model;
using Domain.Model.Product;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceCoreAPI.Controllers;

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
}