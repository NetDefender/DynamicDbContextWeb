using Swashbuckle.AspNetCore.SwaggerUI;

namespace DynamicDbContextWeb.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseSwaggerCustom(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tenant API v1");
                c.DocExpansion(DocExpansion.None);
            });
        }
    }
}
