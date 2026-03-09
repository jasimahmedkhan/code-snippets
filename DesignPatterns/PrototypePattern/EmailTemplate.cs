using System.Text.Json;

namespace design_patterns.PrototypePattern;

public class EmailTemplate : IPrototype<EmailTemplate>
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Sender { get; set; }
    public string Recipient { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    
    public EmailTemplate Clone()
    {
        return new EmailTemplate()
        {
            Body = Body,
            Subject = Subject,
            Sender = Sender,
            Recipient = Recipient,
            CreatedAt = CreatedAt
        };
    }
    
    public override string ToString()
    {
        return $"EmailTemplate: Subject={Subject}, Body={Body}, Sender={Sender}, Recipient={Recipient}, CreatedAt={CreatedAt}";
    }
}


public class ProtoTypeTest
{
    public static void Test_Prototype()
    {
        EmailTemplate template = new EmailTemplate();
        template.Subject = "Hello";
        template.Body = "This is a test email.";
        template.Sender = "sender@example.com";
        template.Recipient = "recipient@example.com";
        
        EmailTemplate cloneEmail = template.Clone();
        cloneEmail.Subject = "Hello, again!";
        cloneEmail.Body = "This is a different email.";
        cloneEmail.Sender = "new_sender@example.com";
        cloneEmail.Recipient = "new_recipient@example.com";

        Console.WriteLine("");
        Console.WriteLine(template.ToString());
        Console.WriteLine(cloneEmail.ToString());
        
    }
}
