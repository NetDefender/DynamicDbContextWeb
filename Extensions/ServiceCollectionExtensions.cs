using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;

namespace DynamicDbContextWeb.Extensions
{
    /// <summary>
    /// Filtro para que salgan las descripciones de los enumerados
    /// </summary>
    /// <seealso cref="IDocumentFilter" />
    internal sealed class SwaggerAddEnumDescriptions : IDocumentFilter
    {
        #region fields
        private Dictionary<string, Type> _enumTypes;
        #endregion

        #region methods            
        /// <summary>
        /// Genera comentarios extendidos para los enumerados
        /// </summary>
        /// <param name="swaggerDoc">The swagger document.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (_enumTypes == null)
            {
                _enumTypes = typeof(Startup).Assembly.GetTypes().Where(t => t.IsEnum)
                    .ToDictionary(t => t.Namespace + "." + t.Name);
            }

            foreach (var property in swaggerDoc.Components.Schemas.Where(x => x.Value?.Enum?.Count > 0))
            {
                IList<IOpenApiAny> propertyEnums = property.Value.Enum;
                if (propertyEnums != null && propertyEnums.Count > 0)
                {
                    property.Value.Description += GenerateEnumDescriptions(propertyEnums, property.Key);
                }
            }

            foreach (var pathItem in swaggerDoc.Paths.Values)
            {
                GenerateEnumParameters(pathItem.Operations, swaggerDoc);
            }
        }

        /// <summary>
        /// Generates the enum parameters.
        /// </summary>
        /// <param name="operations">The operations.</param>
        /// <param name="swaggerDoc">The swagger document.</param>
        private void GenerateEnumParameters(IDictionary<OperationType, OpenApiOperation> operations, OpenApiDocument swaggerDoc)
        {
            if (operations == null)
            {
                return;
            }

            foreach (var oper in operations)
            {
                foreach (var param in oper.Value.Parameters)
                {
                    if (param.Schema?.Reference?.Id != null && swaggerDoc.Components.Schemas.TryGetValue(param.Schema.Reference.Id, out OpenApiSchema paramEnum)
                        && paramEnum.Enum?.Count > 0)
                    {
                        param.Description += paramEnum.Description;
                    }
                }
            }
        }

        /// <summary>
        /// Generates the enum descriptions.
        /// </summary>
        /// <param name="enums">The enums.</param>
        /// <param name="propretyTypeName">Name of the proprety type.</param>
        /// <returns></returns>
        private string GenerateEnumDescriptions(IList<IOpenApiAny> enums, string propretyTypeName)
        {
            if (!_enumTypes.TryGetValue(propretyTypeName, out Type enumType))
            {
                return null;
            }
            List<string> enumDescriptions = new List<string>();

            foreach (OpenApiInteger enumOption in enums.OfType<OpenApiInteger>())
            {
                int enumInt = enumOption.Value;
                enumDescriptions.Add(string.Format("<b><em>{0}</em></b> = {1}", Enum.GetName(enumType, enumInt), enumInt));
            }

            return string.Join(", ", enumDescriptions);
        }
        #endregion
    }
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConnectionPerTenant(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(serviceProvider =>
            {
                TenantInfo tenant = serviceProvider.GetRequiredService<TenantInfo>();
                string connectionString = configuration.GetConnectionString(tenant.Name);
                DbContextOptions<CascadeDeleteContext> options = new DbContextOptionsBuilder<CascadeDeleteContext>()
                    .UseSqlServer(connectionString)
                    .Options;
                return new CascadeDeleteContext(options);
            });

            return services;
        }

        /// <summary>
        /// Configures the swagger.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddSwaggerCustom(this IServiceCollection services)
        {
            const string BEARER = "Bearer";
            const string APIDOC = "APIDoc.xml";
            const string API_VERSION = "v1";

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(API_VERSION, new OpenApiInfo
                {
                    Version = API_VERSION,
                    Title = $"Tenants API {API_VERSION}",
                    Description = "Tenants Web API",
                    Contact = new OpenApiContact()
                    {
                        Name = "Tenants",
                    }
                });

                c.AddSecurityDefinition(BEARER, new OpenApiSecurityScheme
                {
                    Description = "Autorización básica. Example: \"Authorization: Bearer {base64}\"",
                    Name = "Authorization",
                    Scheme = BEARER,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = BEARER
                        },
                        Scheme = "oauth2",
                        Name = BEARER,
                        In = ParameterLocation.Header,

                    },
                        new List<string>()
                 }
            });

                Func<ApiDescription, string> assembly = api => ((ControllerActionDescriptor)api.ActionDescriptor).ControllerTypeInfo.Assembly.GetName().Name;

                c.OrderActionsBy(api =>
                {
                    ControllerActionDescriptor actionDescriptor = (ControllerActionDescriptor)api.ActionDescriptor;
                    string assemblyName = assembly(api);
                    return assemblyName + actionDescriptor.ControllerName + actionDescriptor.ActionName;
                });

                c.TagActionsBy(api => new List<string>
            {
                assembly(api)
            });

                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, APIDOC);
                c.IncludeXmlComments(filePath);
                c.CustomSchemaIds(t => t.FullName);
                c.DocumentFilter<SwaggerAddEnumDescriptions>();
            });
        }
    }


}
