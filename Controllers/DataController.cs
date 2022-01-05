namespace DynamicDbContextWeb.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//[Authorize()]
[Route("[controller]")]
[ApiController]
public sealed class DataController : ControllerBase
{
    public DataController(CascadeDeleteContext context)
    {
        Context = context;
    }

    public CascadeDeleteContext Context
    {
        get;
    }

    
    [HttpGet(nameof(Calculate))]
    public string Calculate()
    {
        return Context.ParentTables.FirstOrDefault()?.Name;
    }
}
