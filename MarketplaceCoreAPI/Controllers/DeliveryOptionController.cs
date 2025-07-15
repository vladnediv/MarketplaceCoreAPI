using System.Threading.Tasks;
using BLL.Service;
using Domain.Model.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryOptionController : Controller
    {
        private readonly DeliveryOptionService _service;

        public DeliveryOptionController(DeliveryOptionService service)
        {
            _service = service;
        }

        [HttpPost("AddDeliveryOption")]
        public async Task<IActionResult> AddDeliveryOption([FromBody]DeliveryOption entity)
        {
            
        }
        
    }
}
