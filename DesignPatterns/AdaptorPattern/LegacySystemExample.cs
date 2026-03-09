using System.Xml.Linq;

namespace design_patterns.AdaptorPattern;

// LEGACY SYSTEM — old code from 2008, uses XML, can't touch it
public class LegacyOrderRepository
{
    public string GetOrderXml(int orderId)
    {
        return $"<order><id>{orderId}</id><status>SHIPPED</status><total>99.99</total></order>";
    }

    public void SaveOrderXml(string xml)
    {
        Console.WriteLine("Saving order to old DB...");
        /* saves XML to old DB */
    }
}

public class Order
{
    public int Id { get; set; }
    public string Status { get; set; }
    public decimal Total { get; set; }
}

// MODERN Target Interface - what your new system expects
public interface IOrderRepository
{
    Order GetById(int orderId);
    void Save(Order order);
}

// ADAPTER — translates modern interface to legacy XML system
public class LegacyOrderAdaptor : IOrderRepository
{
    private readonly LegacyOrderRepository _legacyRepo;

    public LegacyOrderAdaptor(LegacyOrderRepository legacyRepo)
    {
        _legacyRepo = legacyRepo;
    }

    public Order GetById(int orderId)
    {
        // Translate: XML string → modern Order object
        string xml = _legacyRepo.GetOrderXml(orderId);
        var doc = XDocument.Parse(xml);

        return new Order
        {
            Id = int.Parse(doc.Root.Element("id").Value),
            Status = doc.Root.Element("status").Value,
            Total = decimal.Parse(doc.Root.Element("total").Value)
        };
    }

    public void Save(Order order)
    {
        // Translate: modern Order object → XML string
        string xml = $"<order><id>{order.Id}</id><status>{order.Status}</status><total>{order.Total}</total></order>";
        _legacyRepo.SaveOrderXml(xml);
        
    }
}

public class OrderAnalyticsService
{
    private readonly IOrderRepository _orderRepo;
    
    public OrderAnalyticsService(IOrderRepository orderRepo)
    {
        _orderRepo = orderRepo;
    }
    
    public decimal GetOrderTotal(int orderId)
    {
        Order order = _orderRepo.GetById(orderId);
        return order.Total;
    }
}