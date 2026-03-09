namespace design_patterns.PrototypePattern;

public class DocumentTemplate : IPrototype<DocumentTemplate>
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public string Header { get; set; }
    public string Footer { get; set; }
    public Dictionary<string, string> MetaData { get; set; } = new();
    public List<string> Sections { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DocumentTemplate Clone()
    {
        return new DocumentTemplate()
        {
            Title = Title,
            Content = Content,
            Author = Author,
            Header = Header,
            Footer = Footer,
            MetaData = new Dictionary<string, string>(MetaData),
            Sections = new List<string>(Sections),
            CreatedAt = CreatedAt
        };
    }
    
    public override string ToString()
    {
        return $"Title: {Title}, Author: {Author}, Sections: {Sections.Count}, CreatedAt: {CreatedAt}, Header: {Header}, Footer: {Footer}";
    }
    
}


public class DocumentExampleTest
{
    public static void Test_DocumentExample()
    {
        var invoiceTemplate = new DocumentTemplate()
        {
            Title    = "Invoice Template",
            Content  = "This is the content of the invoice.",
            Author   = "Jackson",
            Header   = "Company Inc.",
            Footer   = "Thank you for your business",
            Sections = new List<string> { "Billing Info", "Items", "Total" },
            MetaData = new Dictionary<string, string> { ["type"] = "invoice" }
        };
        
        var clonedInvoiceTemplate = invoiceTemplate.Clone();
        clonedInvoiceTemplate.Title = "Cloned Invoice Template";
        Console.WriteLine($"Original Invoice Template: {invoiceTemplate}");
        Console.WriteLine($"Cloned Invoice Template: {clonedInvoiceTemplate}");
        
    }
}