namespace design_patterns.FactoryPattern;

// Abstract Factory - Families of Related Objects

// Product Interfaces - two families: Light and Dark Theme
public interface IButton
{
    void Render();
}

public interface ITextBox
{
    void Render();
}

public interface ICheckBox
{
    void Render();
}

public interface IRadioButton
{
    void Render();
}

// Concrete Product - Light Theme Family
public class LightButton : IButton
{
    public void Render()
    {
        Console.WriteLine("Light Button");
    }
}

public class LightTextBox : ITextBox
{
    public void Render()
    {
        Console.WriteLine("Light TextBox");
    }
}

public class LightCheckBox : ICheckBox
{
    public void Render()
    {
        Console.WriteLine("Light CheckBox");
    }
}

public class LightRadioButton : IRadioButton
{
    public void Render()
    {
        Console.WriteLine("Light RadioButton");
    }
}

// Concrete Product - Dark Theme Family
public class DarkButton : IButton
{
    public void Render()
    {
        Console.WriteLine("Dark Button");
    }
}

public class DarkTextBox : ITextBox
{
    public void Render()
    {
        Console.WriteLine("Dark TextBox");
    }
}

public class DarkCheckBox : ICheckBox
{
    public void Render()
    {
        Console.WriteLine("Dark CheckBox");
    }
}

public class DarkRadioButton : IRadioButton
{
    public void Render()
    {
        Console.WriteLine("Dark RadioButton");
    }
}

public interface IThemeFactory
{
    public IButton CreateButton();
    public ITextBox CreateTextBox();
    public ICheckBox CreateCheckBox();
    public IRadioButton CreateRadioButton();
}

public class LightThemeFactory : IThemeFactory
{
    public IButton CreateButton()
    {
        return new LightButton();
    }

    public ITextBox CreateTextBox()
    {
        return new LightTextBox();
    }

    public ICheckBox CreateCheckBox()
    {
        return new LightCheckBox();
    }

    public IRadioButton CreateRadioButton()
    {
        return new LightRadioButton();
    }
}

public class DarkThemeFactory : IThemeFactory
{
    public IButton CreateButton()
    {
        return new DarkButton();
    }

    public ITextBox CreateTextBox()
    {
        return new DarkTextBox();
    }

    public ICheckBox CreateCheckBox()
    {
        return new DarkCheckBox();
    }

    public IRadioButton CreateRadioButton()
    {
        return new DarkRadioButton();
    }
}

public class LoginForm
{
    private readonly IButton _submitButton;
    private readonly ITextBox _username;
    private readonly ITextBox _password;
    private readonly ICheckBox _rememberMe;
    private readonly IRadioButton _loginMethod;

    // public LoginForm(ThemeSelector selector, string userTheme)
    public LoginForm(IThemeFactory themeFactory)
    {
        // All components come from the same factory = consistent (family) UI
        _submitButton = themeFactory.CreateButton();
        _username = themeFactory.CreateTextBox();
        _password = themeFactory.CreateTextBox();
        _rememberMe = themeFactory.CreateCheckBox();
        _loginMethod = themeFactory.CreateRadioButton();
    }

    public void Render()
    {
        _submitButton.Render();
        _username.Render();
        _password.Render();
        _rememberMe.Render();
        _loginMethod.Render();
    }
}

public class ThemeSelector
{
    private readonly LightThemeFactory _light = new();
    private readonly DarkThemeFactory _dark = new();

    public IThemeFactory GetFactory(string userTheme)
    {
        return userTheme == "dark" ? _dark : _light;
    }
}

/*
 * DI Container
 *
 * services.AddSingleton<ThemeSelector>();

 * When it makes sense: When the theme depends on something you only know at runtime — user settings, OS preference,
   a database value. The ThemeSelector bridges the gap between startup-time registration and runtime decisions.
 * The tradeoff: Slightly more indirection. You have an extra class. But you keep coding against IThemeFactory which 
   means your LoginForm doesn't care which factory it gets — it just uses whatever the selector returns.
   
 * But when our choice is Fixed and we know which theme to use at startup, we can register the factory directly.
   services.AddSingleton<LightThemeFactory>();
   services.AddSingleton<DarkThemeFactory>();
   
 * services.AddTransient<LoginForm>();
 */

public class AbstractFactoryTests
{
    public static void TestAbstractFactory()
    {
        
        LoginForm lightLoginForm = new LoginForm(new LightThemeFactory());
        lightLoginForm.Render();
        
        LoginForm darkLoginForm = new LoginForm(new DarkThemeFactory());
        darkLoginForm.Render();
    }
}








