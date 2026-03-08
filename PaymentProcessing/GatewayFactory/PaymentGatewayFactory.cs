using PaymentProcessing.Mollie;
using PaymentProcessing.PayPal;
using PaymentProcessing.Services;
using PaymentProcessing.Stripe;

namespace PaymentProcessing.GatewayFactory;

public class PaymentGatewayFactory : IPaymentGatewayFactory
{
    private readonly IServiceProvider _serviceProvider;
    
    public PaymentGatewayFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IPaymentGateway Create(string paymentProvider)
    {
        return paymentProvider.ToLower() switch
        {
            "stripe" => _serviceProvider.GetRequiredService<StripeAdapter>(),
            "mollie" => _serviceProvider.GetRequiredService<MollieAdapter>(),
            "googlepay" => _serviceProvider.GetRequiredService<GooglePayAdapter>(),
            "paypal" => _serviceProvider.GetRequiredService<PayPalAdapter>(),
            // "applepay"  => _serviceProvider.GetRequiredService<ApplePayAdapter>(),
            _ => throw new NotSupportedException($"Payment provider {paymentProvider} not supported")
        };
    }
}