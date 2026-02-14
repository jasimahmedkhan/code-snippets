namespace program_snippets.Generics;

public class Stack<T>
{

    private List<T> _items;

    public Stack()
    {
        _items = new List<T>();
    }

    public void Push(T item)
    {
        _items.Add(item);
    }

    // Even better: TryPop pattern (like Dictionary.TryGetValue)
    public bool TryPop(out T item)
    {
        if (IsEmpty())
        {
            item = default(T);
            return false;
        }
        item = Pop();
        return true;
    }

    private T Pop()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Cannot pop from an empty stack");
        }
        var item = _items[_items.Count - 1];
        _items.RemoveAt(_items.Count -1 );
        return item;
    }

    public bool TryPeek(out T item)
    {
        if (IsEmpty())
        {
            item = default(T);
            return false;
        }

        item = Peek();
        return true;
    }

    private T Peek()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Cannot peek at an empty stack");
        }
        return _items[_items.Count -1];
    }
    
    public bool IsEmpty()
    {
        return _items.Count == 0;
    }

    public void Clear()
    {
        _items.Clear();
    }
    
    public int Count()
    {
        return _items.Count;
    }
}

public class StackTests
{
    public static void TestStack()
    {
        Stack<int> stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        stack.Push(4);
        
        stack.TryPeek(out int top);
        Console.WriteLine(top);
        
        stack.TryPop(out int popped);
        Console.WriteLine(popped);
        
        stack.TryPeek(out top);
        Console.WriteLine(top);
    }
}