using Microsoft.OpenApi;
using PaymentProcessing;
using PaymentProcessing.GatewayFactory;
using PaymentProcessing.Mollie;
using PaymentProcessing.PayPal;
using PaymentProcessing.Services;
using PaymentProcessing.Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Payment Processing API",
        Version = "v1",
        Description = "API for processing payments with Stripe, Mollie, PayPal, and Google Pay"
    });
});


// // Approach 1: Register one by one.
// builder.Services.AddScoped<IPaymentGateway, StripeAdapter>();
// builder.Services.AddScoped<IPaymentGateway, MollieAdapter>(); // drawback of this approach : only the last one gets registered.


// Approach 2: Keyed Services (.NET 8+) — BEST MODERN APPROACH
// Step 1: Register implementations Adaptors and Clients
builder.Services.AddScoped<StripeAPI>();
builder.Services.AddScoped<MollieClient>();
builder.Services.AddScoped<StripeAdapter>();
builder.Services.AddScoped<MollieAdapter>();
builder.Services.AddScoped<PayPalAdapter>();
builder.Services.AddScoped<GooglePayAdapter>();

// Step 2: Register with keys
builder.Services.AddKeyedScoped<IPaymentGateway, StripeAdapter>(PaymentProvider.Stripe);
builder.Services.AddKeyedScoped<IPaymentGateway, MollieAdapter>(PaymentProvider.Mollie);
builder.Services.AddKeyedScoped<IPaymentGateway, PayPalAdapter>(PaymentProvider.PayPal);
builder.Services.AddKeyedScoped<IPaymentGateway, GooglePayAdapter>(PaymentProvider.GooglePay);


// // Approach 3: Factory Pattern + DI — BEST PRE-.NET 8 APPROACH
// // Register in Program.cs — register each adapter + the factory
// builder.Services.AddScoped<StripeAdapter>();
// builder.Services.AddScoped<MollieAdapter>();
// builder.Services.AddScoped<PayPalAdapter>();
// builder.Services.AddScoped<GooglePayAdapter>();
// builder.Services.AddScoped<IPaymentGatewayFactory, PaymentGatewayFactory>();


// // Approach 4:  Dictionary Registration — Clean Alternative
// builder.Services.AddScoped<StripeAdapter>();
// builder.Services.AddScoped<MollieAdapter>();    
// builder.Services.AddScoped<PayPalAdapter>();
// builder.Services.AddScoped<GooglePayAdapter>();
//
// // Register dictionary as factory delegate
// builder.Services.AddScoped<IDictionary<string, IPaymentGateway>>(sp => new Dictionary<string, IPaymentGateway>()
// {
//     ["mollie"] = sp.GetRequiredService<MollieAdapter>(),
//     ["paypay"] = sp.GetRequiredService<PayPalAdapter>(),
//     ["stripe"] = sp.GetRequiredService<StripeAdapter>(),
//     ["googlepay"] = sp.GetRequiredService<GooglePayAdapter>()
// });



builder.Services.AddScoped<CheckoutService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Processing API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();