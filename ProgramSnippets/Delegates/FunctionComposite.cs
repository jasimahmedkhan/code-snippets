using program_snippets.Entities;

namespace program_snippets.Delegates;

public static class FuncExtensions
{
    public static Func<T, TResult2> Then<T, TResult1, TResult2>(
//                     ^  ^^^^^^^^      ^  ^^^^^^^^  ^^^^^^^^
//                     |     |          |     |         |
//                     |     |          |     |         Output type of second function
//                     |     |          |     Output type of first function
//                     |     |          Input type
//                     |     Final output type
//                     Input type
        this Func<T, TResult1> first,
//      ^^^^
//      Extension on Func<T, TResult1>
        Func<TResult1, TResult2> second)
//           ^^^^^^^^  ^^^^^^^^
//           Takes TResult1 (output of first) as input
//           Returns TResult2 as output
    {
        return input => second(first(input));
//             ^^^^^    ^^^^^^ ^^^^^
//             T type   second first(input)
//                      func   gives TResult1
//                             second gives TResult2
    }

    // Function Delegate Composition with two arguments.
    public static Func<T, TResult3> Then<T, TResult1, TResult2, TResult3>(
        this Func<T, TResult1> first,
        Func<TResult1, TResult2, TResult3> second,
        TResult2 args)
    {
        return input => second(first(input), args);
    }
}


public static class IntExtensions
{
    // "this" keyword makes it an extension method!
    public static int Double(this int x)
    {
        return x * 2;
    }
}

public class FunctionCompositeTests
{
    public static void TestFunctionComposite()
    {
        Func<User, string> GetFullName = user => $"{user.FirstName} {user.LastName}";
        
        Func<string, string> toUpperCase = s => s.ToUpper();
        Func<string, string> addGreeting = name => $" Hello: {name}";
        
        
        Func<User, string> greetUser = GetFullName.Then(toUpperCase).Then(addGreeting);
        
        User user = new User {FirstName = "John", LastName = "Doe", Age = 20};
        
        string greeting = greetUser(user);
        Console.WriteLine(greeting);
        
        Console.WriteLine(greetUser(user));
        
        
        // Define individual functions
        Func<int, int> addFive = x =>
        {
            Console.WriteLine($"addFive: {x} + 5 = {x + 5}");
            return x + 5;
        };

        Func<int, int> multiplyByTwo = x =>
        {
            Console.WriteLine($"multiplyByTwo: {x} * 2 = {x * 2}");
            return x * 2;
        };

        Func<int, string> toString = x =>
        {
            string result = x.ToString();
            Console.WriteLine($"toString: {x} -> \"{result}\"");
            return result;
        };
        
        Func<int, int, int> add = (i, j) => i + j;

        var pipeline = addFive.Then(multiplyByTwo)
            .Then(toString); 
            //                        ^^^^^^^^
            //                        "addFive" is "this Func<int, int> first"
            //                                ^^^^
            //                                "multiplyByTwo" is "Func<int, int> second"
                    
        var pipe = addFive.Then(multiplyByTwo).Then(add, 10).Then(toString);
        
        Console.WriteLine(pipeline(10));
        // What happens:
        // 1. combined(10) is called
        // 2. first(10) -> addFive(10) -> 15
        // 3. second(15) -> multiplyByTwo(15) -> 30
        // 4. Returns 30
        

        Console.WriteLine(10.Double());
    }
}