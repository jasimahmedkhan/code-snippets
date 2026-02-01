using System.Collections;

namespace design_patterns.IteratorPattern;

public class NumberRange : IEnumerable<int>
{
    private int _start;
    private int _end;
    
    public NumberRange(int start, int end)
    {
        _start = start;
        _end = end;
    }
    
    public IEnumerator<int> GetEnumerator()
    {
        // yield return statements are like return statements, but they can be used inside a loop
        for (int i = _start; i < _end; i++)
        {
            yield return i; // Return the current value of i and pause the execution of the loop
        }
    }

    IEnumerator IEnumerable.GetEnumerator() =>  GetEnumerator();
    
}

public class ProgramNumberRange
{
    public static void Main()
    {
        foreach (var number in new NumberRange(1, 10))
        {
            Console.WriteLine(number);
        }
        
        // Lazy evaluation - only generates number when requested!
       var squares = new NumberRange(1, 10000)
            .Select(x => x * x)
            .Where(x => x > 100)
            .Take(10);
            
        foreach (var square in squares)
        {
            Console.WriteLine(square);
        }
    }
    
}