namespace design_patterns.Interfaces;

// Marker: "This entity needs audit logging"
public interface IAuditable { }

public class Order : IAuditable
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
}

public class Product  // Not auditable
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// Only Order gets audit logs—Product is ignored.
public class AppDbContext //  : DbContext
{
    
//    auditLogger.Log(order)
    
}