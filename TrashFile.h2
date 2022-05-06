global using DynamicDbContextWeb.Extensions;
global using DynamicDbContextWeb.Infraestructure;
global using DynamicDbContextWeb.Models;
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.HttpsPolicy;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.IdentityModel.Tokens;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using Microsoft.AspNetCore.Mvc;

namespace DynamicDbContextWeb;
public sealed partial class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration
    {
        get;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddControllersWithViews();
        services.AddControllers();
        services.AddScoped<TenantInfo>();
        services.AddSingleton(Configuration.Get<Settings>());
        services.AddConnectionPerTenant(Configuration);
        //services.AddAuthentication(options =>
        //  {
        //      options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //      options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //      options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //      options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //  })
        services.AddAuthentication().AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["JwtToken:Issuer"],
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtToken:SecretKey"]))
            };

            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    string userId = context.Principal.Identity.Name;

                    if (string.IsNullOrWhiteSpace(userId))
                    {
                        context.Fail("Unauthorized");
                    }
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                }
            };
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.LoginPath = "/Register";
            options.AccessDeniedPath = "/Error";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // optional
        });

        //AuthorizationPolicy multiSchemePolicy = new AuthorizationPolicyBuilder(
        //    CookieAuthenticationDefaults.AuthenticationScheme,
        //    JwtBearerDefaults.AuthenticationScheme)
        //  .RequireAuthenticatedUser()
        //  .Build();

        //services.AddAuthorization(o => o.DefaultPolicy = multiSchemePolicy);
        services.AddSwaggerCustom();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<TenantInfoMiddleware>();

        app.UseSwaggerCustom();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
        });
    }
}
