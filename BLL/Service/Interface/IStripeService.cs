using Stripe;

namespace BLL.Service.Interface;

public interface IStripeService
{
    public Task<string> CreatePaymentIntentAsync(long amount);
}