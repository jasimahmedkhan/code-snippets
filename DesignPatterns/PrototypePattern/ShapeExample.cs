namespace design_patterns.PrototypePattern;

public interface IShape
{
    IShape Clone();
    void Draw();
}

public class ShapeRegistry
{
    private Dictionary<string, IShape> _shapes = new Dictionary<string, IShape>();

    public void Register(string key, IShape shape)
    {
        _shapes[key] = shape;
    }

    public IShape Get(string key)
    {
        if (!_shapes.ContainsKey(key))
        {
            throw new KeyNotFoundException($"Shape with key '{key}' not found.");
        }

        return _shapes[key].Clone();
    }
}

public class Circle : IShape
{
    public int x { get; set; }
    public int y { get; set; }
    public int radius { get; set; }
    public string Color { get; set; }
    

    public IShape Clone()
    {
        return new Circle() { x = x, y = y, radius = radius, Color = Color };
    }

    public void Draw()
    {
        Console.WriteLine($"Drawing Circle at ({x}, {y}) with radius {radius} and color {Color ?? "default"}");
    }
}

public class Rectangle : IShape
{
    public int width { get; set; }
    public int height { get; set; }
    public string Color { get; set; }

    public IShape Clone()
    {
        return new Rectangle()
        {
            height = height,
            width = width,
            Color = Color
        };
    }

    public void Draw()
    {
        Console.WriteLine($"Drawing Rectangle, width: {width}, height: {height} and color {Color ?? "default"}");
    }
}

public class Square : IShape
{
    public int sideLength { get; set; }
    public string Color { get; set; }

    public IShape Clone()
    {
        return new Square()
        {
            sideLength = sideLength,
            Color = Color
        };
    }

    public void Draw()
    {
        Console.WriteLine($"Drawing Square with side length: {sideLength} and color {Color ?? "default"}");
    }
}

public class ShapeTest
{
    public static void Test_Shape()
    {
        var circle = new Circle() { x = 10, y = 20, radius = 5 };
        var rectangle = new Rectangle() { width = 10, height = 20 };
        var square = new Square() { sideLength = 10 };

        circle.Draw();
        rectangle.Draw();
        square.Draw();

        var clonedCircle = circle.Clone();
        clonedCircle.Draw();
        var clonedRectangle = rectangle.Clone();
        clonedRectangle.Draw();
        var clonedSquare = square.Clone();
        clonedSquare.Draw();

        Console.WriteLine("----------------------------------------");

        Console.WriteLine("Cloned shapes are identical to original shapes");
    }

    public static void Test_ShapeRegistry()
    {
        var registry = new ShapeRegistry();

        // Pre-register prototypes (expensive setup done once)
        registry.Register("small-red-circle",   new Circle    { x = 5,  y = 6, radius = 3, Color = "Red"});
        registry.Register("big-blue-circle",    new Circle    {  x = 15,  y = 16, radius = 13, Color = "Blue"});
        registry.Register("green-rect",         new Rectangle { width = 100, height = 50, Color = "Green" });

        Console.WriteLine("----------------------------------------");
        Console.WriteLine("Prototype registry setup complete");
        
        // Clone and customize at will
        var circle1 = (Circle)registry.Get("small-red-circle");
        circle1.x = 10; circle1.y = 20;

        var circle2 = (Circle)registry.Get("small-red-circle"); // Fresh clone!
        circle2.x = 50; circle2.y = 80; // Different position, same config

        circle1.Draw(); // Circle: r=5, color=Red at (10,20)
        circle2.Draw(); // Circle: r=5, color=Red at (50,80)
        
    }
}