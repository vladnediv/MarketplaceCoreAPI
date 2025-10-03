using BLL.Service.Interface;
using Stripe;

namespace BLL.Service.ServiceHelpers;

public class StripeService : IStripeService
{
    public async Task<string> CreatePaymentIntentAsync(long amount)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = amount,
            Currency = "uah",
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true,
            },
        };

        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(options);

        return paymentIntent.ClientSecret;
    }
}