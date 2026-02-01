using System.Collections;

namespace design_patterns.IteratorPattern;

public class Product
{
    public string Name { get; set; }
}

// VIOLATION: Exposing internal structure
public class BadProductCatalog 
{
    public List<Product> Products { get; set; } = new List<Product>();
    
    // Client code directly accesses internal list
    // Problems:
    // 1. Client knows we use List (tight coupling)
    // 2. Client can modify the list directly
    // 3. Can't change internal structure without breaking clients
}



// CORRECT: Hide internal structure with iterator
public class ProductCatalog : IEnumerable<Product> 
{
    private List<Product> _products = new List<Product>(); // Private!
    
    public void AddProduct(Product product) 
    {
        _products.Add(product);
    }
    
    // Return iterator, not internal collection
    public IEnumerator<Product> GetEnumerator() 
    {
        foreach (var product in _products) 
        {
            yield return product;
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class ProgramProductCatalog
{
    public static void Main()
    {
        // Usage
        BadProductCatalog catalog = new BadProductCatalog();
        catalog.Products.Add(new Product { Name = "Product 1" });
        catalog.Products.Add(new Product { Name = "Product 2" });
        catalog.Products.Add(new Product { Name = "Product 3" });
        Console.WriteLine($"The product Count = {catalog.Products.Count}");
        foreach (var product in catalog.Products) // Exposes List<T>
        {
            Console.WriteLine(product.Name);
        }
        catalog.Products.Clear(); // Oops! External modification!
        Console.WriteLine($"The product Count = {catalog.Products.Count}");
        
        // Better Usage
        ProductCatalog productCatalog = new ProductCatalog();
        productCatalog.AddProduct(new Product { Name = "Product 1" });
        productCatalog.AddProduct(new Product { Name = "Product 2" });
        productCatalog.AddProduct(new Product { Name = "Product 3" });
        foreach (var product in productCatalog) // Uses iterator
        {
            Console.WriteLine(product.Name);
        }
        // Internal structure hidden - can change from List to Dictionary later
    }
}