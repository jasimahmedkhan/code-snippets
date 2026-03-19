namespace design_patterns.FactoryPattern;

/*
Four players. Always:
   Product interface — what gets created
   Concrete Products — actual things created
   Abstract Creator — has the factory method + template method
   Concrete Creators — override the factory method
*/

// Product interface — what gets created
public interface IPaymentProcessor
{
    bool ProcessPayment(decimal amount);
    void Refund(decimal amount);
}

// Concrete Products — actual things created
public class StripeProcessor : IPaymentProcessor
{
    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Stripe: Processing payment {amount}");
        return true;
    }

    public void Refund(decimal amount)
    {
        Console.WriteLine($"Stripe: Refunding {amount}");
    }
}

public class PayPalProcessor: IPaymentProcessor
{
    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"PayPal: Processing payment {amount}");
        return true;
    }

    public void Refund(decimal amount)
    {
        Console.WriteLine($"PayPal: Refunding {amount}");
    }
}

public class MollieProcessor: IPaymentProcessor
{
    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Mollie: Processing payment {amount}");
        return true;
    }

    public void Refund(decimal amount)
    {
        Console.WriteLine($"Mollie: Refunding {amount}");
    }
}

// Creator (Abstract Class) - Defines WHEN to use the Factory Method
// Abstract Creator — has the factory method + template method
public abstract class PaymentGateway
{
    // Factory Method - subclasses decide WHICH processor to create; abstract, subclass must override
    public abstract IPaymentProcessor CreatePaymentProcessor();
    
    // Template Method - Defines how to use the Factory Method or
    // uses the factory method internally
    // This method is complete and reusable. It never changes.
    public bool Checkout(decimal amount)
    {
        var processor = CreatePaymentProcessor();  // Calls the Factory Method
        ValidateAmount(amount); // Common logic
        return processor.ProcessPayment(amount);
    }

    private void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be positive", nameof(amount));
        }
    }
}

// Concrete Creators - each decides WHAT to create
public class StripeGateway : PaymentGateway
{
    public override IPaymentProcessor CreatePaymentProcessor()
    {
        return new StripeProcessor(); // StripeGateway creates the StripeProcessor
    }
}

public class PayPalGateway : PaymentGateway
{
    public override IPaymentProcessor CreatePaymentProcessor()
    {
        return new PayPalProcessor(); // PayPalGateway creates the PayPalProcessor
    }
}

public class MollieGateway : PaymentGateway
{
    public override IPaymentProcessor CreatePaymentProcessor()
    {
        return new MollieProcessor(); // MollieGateway creates the MollieProcessor
    }
}


public class FactoryMethodTests
{
    public static void TestFactoryMethod()
    {
        PaymentGateway gateway = new StripeGateway();
        gateway.Checkout(99.9M);
    }
    
    public static void TestFactoryMethodWithPayPal()
    {
        PaymentGateway gateway = new PayPalGateway();
        gateway.Checkout(12.9M);
    }
    
    public static void TestFactoryMethodWithMollie()
    {
        PaymentGateway gateway = new MollieGateway();
        gateway.Checkout(45.9M);
    }
    
}