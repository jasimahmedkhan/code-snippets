using program_snippets.Entities;

public class Variance
{
    public static void GetPersonInfo(out string name, out int age)
    {
        name = "John";
        age = 25;
    }
    
    public static void SetPersonInfo(in string name, in int age)
    {
        Console.WriteLine($"{name} is {age} years old");
    }
    
    // Or as a Method
    public static string GetFullName<T>(T input) where T : IHasName
    {
        return $"{input.FirstName} {input.LastName}";
    }

    
}

public class VarianceTest
{
    public static void TestVariance()
    {
        // Covariance and Contravariance
        string personName;
        int personAge;
        Variance.GetPersonInfo(out personName, out personAge);
        Console.WriteLine($"{personName} is {personAge} years old");
        Variance.SetPersonInfo("James Bond", 19);
        
        // Generic Func Usage:
        // Generic Func that works for ANY type with IHasName!
        Func<T, string> GetFullName<T>() where T : IHasName
        {
            return (input) => $"{input.FirstName} {input.LastName}";
        }
        var person = new Person { FirstName = "John", LastName = "Doe"};
        var customer = new Customer { FirstName = "Erika", LastName = "Klein", CustomerId = "12345"};
        var employee = new Employee { FirstName = "Michelle", LastName = "Obama", Department = "IT"};
        Console.WriteLine(GetFullName<Person>()(person));
        Console.WriteLine(GetFullName<Customer>()(customer));
        Console.WriteLine(GetFullName<Employee>()(employee));
    }
}