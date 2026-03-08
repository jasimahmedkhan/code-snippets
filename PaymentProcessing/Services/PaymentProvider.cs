namespace PaymentProcessing.Services;

public enum PaymentProvider
{
    Stripe,
    Mollie,
    PayPal,
    GooglePay,
    ApplePay,
    AmazonPay
}