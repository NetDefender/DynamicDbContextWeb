namespace DynamicDbContextWeb.Models;

public sealed class LoginCredentials
{
    public string Name
    {
        get;
        set;
    }

    public string Password
    {
        get;
        set;
    }

    public string Domain
    {
        get;
        set;
    }
}
