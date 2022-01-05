namespace DynamicDbContextWeb.Models;
public interface IDeletedEntity
{
    bool IsDeleted
    {
        get;
        set;
    }
}