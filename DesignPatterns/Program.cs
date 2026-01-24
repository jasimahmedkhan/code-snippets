// See https://aka.ms/new-console-template for more information

using design_patterns.ObserverPattern;


Console.WriteLine("=== Observer Pattern Demo ===\n");
        
// Create the subject (thing being observed)
var sensor = new TemperatureSensor();
        
// Create observers (things watching)
var display1 = new TemperatureDisplay("Living Room Display");
var display2 = new TemperatureDisplay("Bedroom Display");
var alert = new TemperatureAlert(30);
        
Console.WriteLine("--- Subscribing Observers ---");
display1.Subscribe(sensor);
display2.Subscribe(sensor);
alert.Subscribe(sensor);
        
Console.WriteLine("\n--- Temperature Changes ---");
sensor.SetTemperature(22);
sensor.SetTemperature(28);
sensor.SetTemperature(35); // This should trigger alert
        
Console.WriteLine("\n--- Display2 Unsubscribes ---");
display2.Unsubscribe();
        
Console.WriteLine("\n--- Temperature Changes (Display2 won't see this) ---");
sensor.SetTemperature(20);
        
Console.WriteLine("\n--- Simulating Sensor Error ---");
sensor.ReportError("Sensor malfunction detected");
        
Console.WriteLine("\n--- Sensor Stops Monitoring ---");
sensor.StopMonitoring();
        
Console.WriteLine("\n--- Trying to set temp after completion (no observers left) ---");
sensor.SetTemperature(25); // Nothing happens
        
Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();