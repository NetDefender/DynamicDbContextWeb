using Microsoft.Extensions.Logging;

namespace DynamicDbContextWeb.Models;

public sealed class Settings
{
    public bool DetailedErrors
    {
        get; set;
    }
    public Logging Logging
    {
        get; set;
    }
    public JwtToken JwtToken
    {
        get; set;
    }
}

public sealed class Logging
{
    public LogLevel LogLevel
    {
        get; set;
    }
}

public sealed class JwtToken
{
    public string SecretKey
    {
        get; set;
    }
    public string Issuer
    {
        get; set;
    }
}
