#nullable disable

namespace DynamicDbContextWeb.Models;
public partial class ChildTable : IStateEntity
{
    public ChildTable()
    {
        GrandChildTables = new HashSet<GrandChildTable>();
    }
    public int IdChild
    {
        get;
        set;
    }
    public int IdParent
    {
        get;
        set;
    }
    public string Name
    {
        get;
        set;
    }
    public virtual ParentTable IdParentNavigation
    {
        get;
        set;
    }
    public virtual ICollection<GrandChildTable> GrandChildTables
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