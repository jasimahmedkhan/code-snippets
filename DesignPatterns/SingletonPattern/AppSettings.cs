using Microsoft.Extensions.Configuration;

namespace design_patterns.SingletonPattern;

public sealed class AppSettings
{
    private static readonly Lazy<AppSettings> _instance = new(() => new AppSettings());

    public string ApiBaseUrl { get; set; }
    public int RequestTimeout { get; set; }
    public bool EnableCaching { get; set; }


    private AppSettings()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        ApiBaseUrl = config["ApiBaseUrl"];
        RequestTimeout = int.Parse(config["RequestTimeout"]);
        EnableCaching = bool.Parse(config["EnableCaching"]);
    }


    public static AppSettings Instance => _instance.Value;
}