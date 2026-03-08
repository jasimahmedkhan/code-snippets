using PaymentProcessing.Services;

namespace PaymentProcessing.GatewayFactory;

public interface IPaymentGatewayFactory
{
    IPaymentGateway Create(string paymentProvider);
}