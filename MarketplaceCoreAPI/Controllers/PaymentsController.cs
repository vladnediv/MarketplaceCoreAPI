using System.IO;
using System.Threading.Tasks;
using BLL.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace MarketplaceCoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : Controller
{
    private readonly IStripeService _stripeService;

    public PaymentsController(IStripeService stripeService)
    {
        _stripeService = stripeService;
    }
    [HttpPost("create-payment-intent")]
    public async Task<IActionResult> CreatePaymentIntent(paymentModel model)
    {
        var res = await _stripeService.CreatePaymentIntentAsync(model.amount);
        
        return Ok(new { clientSecret = res });
    }
    
    public class paymentModel
    {
        public long amount { get; set; }
    }
}