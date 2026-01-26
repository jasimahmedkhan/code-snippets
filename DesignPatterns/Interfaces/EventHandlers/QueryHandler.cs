using Microsoft.AspNetCore.Mvc;

namespace design_patterns.Interfaces.EventHandlers;

// Step 1: Define marker interface for queries
public interface IQuery<TResult>
{
}
// This says: "I am a query that returns TResult"

// Step 2: Define handler interface with constraint
public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult> // ← CONSTRAINT: Query must match result
{
    Task<TResult> HandleAsync(TQuery query);
}

// What the constraint does: It forces TQuery to implement IQuery<TResult>,
// which guarantees the query and result types match.

// Define domain model
public class Order
{
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
}

// Define what data we want back
public class OrderDto
{
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
}

// Define the query - it implements IQuery<OrderDto>
public record GetOrderByIdQuery(Guid OrderId) : IQuery<OrderDto>;
//                                                      ^^^^^^^^
//                                              This says: "I return OrderDto"

// Define interface for repository that directly returns Order
public interface IOrderRepository
{
    Task<Order> GetOrderByIdAsync(Guid orderId);
}

public class OrderRepository : IOrderRepository
{
    public Task<Order> GetOrderByIdAsync(Guid orderId)
    {
        // Simulate database query
        
        var order = new Order
        {
            OrderId = orderId,
            TotalAmount = 100,
            Status = "Pending"
        };
        return Task.FromResult(order);
    }
}

// Creating Handler
public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDto> HandleAsync(GetOrderByIdQuery query)
    {
        var order = await _orderRepository.GetOrderByIdAsync(query.OrderId);

        return new OrderDto()
        {
            OrderId = order.OrderId,
            TotalAmount = order.TotalAmount,
            Status = order.Status
        };
    }
}

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IQueryHandler<GetOrderByIdQuery, OrderDto> _handler;

    // Constructor injection - DI container fills these automatically
    public OrderController(IQueryHandler<GetOrderByIdQuery, OrderDto> handler)
    {
        _handler = handler;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetById(Guid id)
    {
        // Create query
        var query = new GetOrderByIdQuery(id);

        // Execute query
        var result = await _handler.HandleAsync(query);
        //   ^^^^^^ 
        //   Compiler KNOWS this is OrderDto (not object or anything else)

        return Ok(result);
    }
}