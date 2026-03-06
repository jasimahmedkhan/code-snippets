using Microsoft.Extensions.DependencyInjection;

namespace design_patterns.FactoryPattern;

// Product Interface
public interface INotificationService
{
    public void Send(string message, string recipient);
}

// Concrete Product
public class EmailNotificationService : INotificationService
{
    public void Send(string message, string recipient)
    {
        Console.WriteLine($"Sending email to {recipient}: {message}");
    }
}

public class PushNotificationService : INotificationService
{
    public void Send(string message, string recipient)
    {
        Console.WriteLine($"Sending push notification to {recipient}: {message}");
    }
}

public class SmsNotificationService : INotificationService
{
    public void Send(string message, string recipient)
    {
        Console.WriteLine($"Sending SMS to {recipient}: {message}");
    }
}

public interface INotificationFactory
{
    INotificationService Create(string channel);
}

public class NewNotificationFactory : INotificationFactory
{
    private readonly IServiceProvider _serviceProvider;
    
    public NewNotificationFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public INotificationService Create(string channel)
    {
        return channel switch
        {
            "email" => _serviceProvider.GetRequiredService<EmailNotificationService>(),
            "push" => _serviceProvider.GetRequiredService<PushNotificationService>(),
            "sms" => _serviceProvider.GetRequiredService<SmsNotificationService>(),
            _ => throw new ArgumentException("Invalid service type", nameof(channel))
        };
    }
}

/*
 DI Container Registration
 services.AddSingleton<INotificationFactory, NewNotificationFactory>();
 services.AddTransient<EmailNotificationService>();
 services.AddTransient<PushNotificationService>();
 services.AddTransient<SmsNotificationService>();
 */


// Simple Factory
public static class NotificationFactory
{
    public static INotificationService Create(string channel)
    {
        return channel switch
        {
            "email" => new EmailNotificationService(),
            "push" => new PushNotificationService(),
            "sms" => new SmsNotificationService(),
            _ => throw new ArgumentException("Invalid service type", nameof(channel))
        };
    }
}

public class OrderService
{
    private readonly INotificationService _notificationService;
    
    public OrderService(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public void NotifyCustomer(string channel, string orderId)
    {
        // No 'new', no conditional, no coupling to concrete types
        var service = NotificationFactory.Create(channel);
        _notificationService.Send($"Order {orderId} is ready for pickup.", "user@example.com");
    }
    
}

public static class SimpleFactoryTests
{
    public static void TestSimpleFactory()
    {
        INotificationService service = NotificationFactory.Create("email");
        Console.WriteLine("");
        service.Send("Your order is ready!", "user@example.com");
    }
}