#nullable disable

namespace DynamicDbContextWeb.Models;

public partial class ParentTable : IStateEntity
{
    public ParentTable()
    {
        ChildTables = new HashSet<ChildTable>();
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
    public virtual ICollection<ChildTable> ChildTables
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
