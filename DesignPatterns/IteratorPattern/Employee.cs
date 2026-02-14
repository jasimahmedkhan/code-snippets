using System.Collections;

namespace design_patterns.IteratorPattern;

// Using C#'s built-in iterator interfaces --->  IEnumerable/IEnumerator
public class Employee
{
    public string Name { get; set; }
    public decimal Salary { get; set; }
}

public class Company : IEnumerable<Employee>
{
    private List<Employee> _employees = new List<Employee>();

    public void Add(Employee employee)
    {
        _employees.Add(employee);
    }

    // IEnumerable<T> requires GetEnumerator()
    public IEnumerator<Employee> GetEnumerator()
    {
        return new EmployeeIterator(_employees);
    }

    // Non-generic version required by IEnumerable
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class EmployeeIterator : IEnumerator<Employee>
    {
        private List<Employee> _employees;
        private int _currentIndex = -1; // Index of current employee Before the first MoveNext() call

        public EmployeeIterator(List<Employee> employees)
        {
            _employees = employees;
        }

        public Employee Current
        {
            get
            {
                if (_currentIndex < 0 || _currentIndex >= _employees.Count) 
                    throw new InvalidOperationException();
                return _employees[_currentIndex];
            }
        }
        
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_currentIndex >= _employees.Count - 1) return false;
            _currentIndex++;
            return true;
        }

        public void Reset()
        {
            _currentIndex = -1;
        }

        public void Dispose()
        {
            
        }
    }
}

public class ProgramEmployee
{
    public static void Main()
    {
        Company company = new Company();
        company.Add(new Employee { Name = "John", Salary = 100000 });
        company.Add(new Employee { Name = "Mary", Salary = 50000 });
        company.Add(new Employee { Name = "Bob", Salary = 70000 });
        
        // for-each automatically uses GetEnumerator where it Iterates over employees
        foreach (var employee in company)
        {
            Console.WriteLine($"{employee.Name} earns {employee.Salary}");
        }
        
        // Equivalent manual iteration, need to manually dispose the enumerator object.
        using var iterator = company.GetEnumerator();
        while (iterator.MoveNext())
        {
            Console.WriteLine($"{iterator.Current.Name} earns {iterator.Current.Salary}");
        }
    }
}