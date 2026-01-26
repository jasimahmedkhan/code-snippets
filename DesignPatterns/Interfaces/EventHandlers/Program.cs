using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace design_patterns.Interfaces.EventHandlers;

public class Program
{
    /*
 *1. User makes HTTP request → POST /orders

   2. ASP.NET Core needs to create OrderController

   3. DI Container sees constructor needs:
      - EventHandler<OrderPlacedEvent>
      - IQueryHandler<GetOrderByIdQuery, OrderDto>

   4. DI Container looks up registrations:
      - "EventHandler<OrderPlacedEvent> → create OrderPlacedEventHandler"
      - "IQueryHandler<...> → create GetOrderByIdQueryHandler"

   5. But wait! OrderPlacedEventHandler needs dependencies:
      - IValidator<OrderPlacedEvent>
      - IUnitOfWork
      - IEventPublisher
      - IOrderRepository

   6. DI Container creates those too (recursively)

   7. DI Container creates OrderController with all dependencies

   8. Request executes successfully
 *
 */

    // What happens: DI container stores a mapping: "When someone asks for EventHandler<OrderPlacedEvent>,
    // create and return OrderPlacedEventHandler".
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // REGISTRATION PHASE: Tell DI container what to create
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        // builder.Services.AddScoped<IEventPublisher, EventPublisher>();

        // Register generic handlers with constraints
        // builder.Services.AddScoped<
        //     EventHandler<OrderPlacedEvent>,      // Interface (what)
        //     OrderPlacedEventHandler>();           // Implementation (how)
        //  ^^^^^^^^^^^^^^^^^^^^^^^^
        //  The constraint ensures OrderPlacedEvent : IEvent

        builder.Services.AddScoped<
            IQueryHandler<GetOrderByIdQuery, OrderDto>, // Interface
            GetOrderByIdQueryHandler>(); // Implementation

        var app = builder.Build();
        app.Run();
    }
}