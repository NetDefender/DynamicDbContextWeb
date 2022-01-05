using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace DynamicDbContextWeb.Infraestructure;
public class TenantInfoMiddleware
{
    private readonly RequestDelegate _next;

    public TenantInfoMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            TenantInfo tenantInfo = context.RequestServices.GetRequiredService<TenantInfo>();
            tenantInfo.Name = context.User.Claims.First(c => c.Type == ClaimTypes.UserData).Value;
        }

        await _next(context).ConfigureAwait(false);
    }
}