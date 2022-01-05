#nullable disable

namespace DynamicDbContextWeb.Models;
public partial class GrandChildTable : IStateEntity
{
    public int IdGrandChild
    {
        get;
        set;
    }
    public int IdChild
    {
        get;
        set;
    }
    public string Name
    {
        get;
        set;
    }

    public virtual ChildTable IdChildNavigation
    {
        get;
        set;
    }
    public bool IsDeleted
    {
        get;
        set;
    }
    public bool IsAdded
    {
        get;
        set;
    }
    public bool IsModified
    {
        get;
        set;
    }
}