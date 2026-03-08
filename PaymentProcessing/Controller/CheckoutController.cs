using Microsoft.AspNetCore.Mvc;
using PaymentProcessing.Services;

namespace PaymentProcessing.Controller;

[ApiController]
[Route("api")]
public class CheckoutController : ControllerBase
{
    private readonly CheckoutService _checkoutService;
    
    public CheckoutController(CheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    [HttpPost("checkout")]
    public ActionResult Checkout([FromBody] CheckoutRequest request)
    {
        // request.PaymentProvider = "Stripe" / "Mollie" / "PayPal" from UI
        var result = _checkoutService.ProcessPayment(request.PaymentProvider, request.Amount);
        return Ok(result);
    }
    
    [HttpGet("status")]
    public ActionResult Status()
    {
        // request.PaymentProvider = "Stripe" / "Mollie" / "PayPal" from UI
        return Ok("Hello from the status check endpoint");
    }
}

public record CheckoutRequest(string PaymentProvider, decimal Amount, string Currency);