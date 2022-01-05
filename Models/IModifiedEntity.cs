namespace DynamicDbContextWeb.Models;
public interface IModifiedEntity
{
    bool IsModified
    {
        get;
        set;
    }
}