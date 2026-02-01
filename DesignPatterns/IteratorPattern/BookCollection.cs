namespace design_patterns.IteratorPattern;

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
}

// Iterator Interface (i.e., IEnumerator in C# )
public interface IIterator<T>
{
    bool HasNext();
    T Next();
    void Reset();
}

// Aggregate Interface (i.e., IEnumerable in C# )
public interface ICollection<T>
{
    IIterator<T> CreateIterator();
}

// Concrete Iterator implements the traversal logic and tracks the current position in the collection.
public class BookIterator : IIterator<Book>
{
    private BookCollection _collection;
    private int _currentIndex = 0;

    public BookIterator(BookCollection collection)
    {
        _collection = collection;
    }
    
    
    public bool HasNext()
    {
        return _currentIndex < _collection.Count;
    }

    public Book Next()
    {
        if(!HasNext())
            throw new InvalidOperationException();
        return _collection[_currentIndex++];
    }

    public void Reset()
    {
        _currentIndex = 0;
    }
}

// Concrete Aggregate
public class BookCollection : ICollection<Book>
{
    private List<Book> _books = new List<Book>();

    public void Add(Book book)
    {
        _books.Add(book);
    }
    
    public int Count => _books.Count;
    
    public Book this[int index] => _books[index];
    
    public IIterator<Book> CreateIterator()
    {
        return new BookIterator(this);
    }
}

public class ProgramCollection
{
    public static void Main()
    {
        BookCollection library = new BookCollection();

        library.Add(new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien" });
        library.Add(new Book { Title = "The Lord of the Rings", Author = "J.R.R. Tolkien" });
        library.Add(new Book { Title = "The Hitchhiker's Guide to the Galaxy", Author = "Douglas Adams" });
        library.Add(new Book { Title = "The Alchemist", Author = "Paulo Coelho" });
        library.Add(new Book { Title = "The Da Vinci Code", Author = "Leo Tolstoy" });

        IIterator<Book> iterator = library.CreateIterator();
        while (iterator.HasNext())
        {
            Book book = iterator.Next();
            Console.WriteLine($"{book.Title} by {book.Author}");
        }

    }
}