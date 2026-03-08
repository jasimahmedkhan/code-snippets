using PaymentProcessing.GatewayFactory;
using PaymentProcessing.Result;

namespace PaymentProcessing.Services;

// // Approach 1: For Injecting single implementation of IPaymentGateway manually.
// public class CheckoutService
// {
//     private readonly IPaymentGateway PaymentExample;
//
//     public CheckoutService(IPaymentGateway paymentExample)
//     {
//         PaymentExample = paymentExample;
//     }
//
//     public bool ProcessPayment(string customerId, decimal amount)
//     {
//         var result = PaymentExample.Charge(customerId, amount, "EUR");
//         return result.Success;
//     }
//
//     public bool Refund(string transactionId, decimal amount)
//     {
//         return PaymentExample.Refund(transactionId, amount);
//     }
// }


// Approach 2: Keyed Services (.NET 8+) — BEST MODERN APPROACH
// Resolve dynamically at Runtime using IKeyedServiceProvider 
public class CheckoutService
{
    private readonly IServiceProvider _serviceProvider;

    public CheckoutService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TransactionResult ProcessPayment(string selectProvider, decimal amount)
    {
        var gateway = GetGatewayProvider(selectProvider);
        return gateway.Charge(Guid.NewGuid().ToString(), amount, "EUR");
    }

    public bool Refund(string selectProvider, string transactionId, decimal amount)
    {
        var gateway = GetGatewayProvider(selectProvider);
        return gateway.Refund(transactionId, amount);
    }

    private IPaymentGateway GetGatewayProvider(string selectProvider)
    {
        // Parse user's selection to enum key
        var providerKey = Enum.Parse<PaymentProvider>(selectProvider, ignoreCase: true);
        return _serviceProvider.GetRequiredKeyedService<IPaymentGateway>(providerKey);
    }
}


// // Approach 3: Factory Pattern + DI — BEST PRE-.NET 8 APPROACH
// public class CheckoutService
// {
//     private readonly IPaymentGatewayFactory _gatewayFactory;
//
//     public CheckoutService(IPaymentGatewayFactory gatewayFactory)
//     {
//         _gatewayFactory = gatewayFactory;
//     }
//
//     public TransactionResult ProcessPayment(string selectProvider, decimal amount)
//     {
//         var gateway = _gatewayFactory.Create(selectProvider);
//         return gateway.Charge(Guid.NewGuid().ToString(), amount, "EUR");
//     }
//
//     public bool Refund(string selectProvider, string transactionId, decimal amount)
//     {
//         var gateway = _gatewayFactory.Create(selectProvider);
//         return gateway.Refund(transactionId, amount);
//     }
// }


// //  Approach 4: Dictionary Registration — Clean Alternative
// public class CheckoutService
// {
//     private readonly IDictionary<string, IPaymentGateway> _gateways;
//
//     public CheckoutService(IDictionary<string, IPaymentGateway> gateways)
//     {
//         _gateways = gateways;
//     }
//
//     public TransactionResult ProcessPayment(string selectProvider, decimal amount)
//     {
//         if (!_gateways.TryGetValue(selectProvider, out var gateway))
//         {
//             throw new NotSupportedException($"Provider '{selectProvider}' not supported");
//         }
//         return gateway.Charge(Guid.NewGuid().ToString(), amount, "EUR");
//     }
//
//     public bool Refund(string selectProvider, string transactionId, decimal amount)
//     {
//         if (!_gateways.TryGetValue(selectProvider, out var gateway))
//         {
//             throw new NotSupportedException($"Provider '{selectProvider}' not supported");
//         }
//         return gateway.Refund(transactionId, amount);
//     }
//     
// }