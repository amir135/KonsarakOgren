using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Autofac;
using System.Linq;
using Autofac.Extensions.DependencyInjection;
using KonOgren.Infrastructure.AutoMappers;
using KonOgren.Infrastructure.StaticContents;
using KonOgren.DataAccess;

namespace KonOgren.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            StaticConfiguration.ConnectionString = Configuration.GetConnectionString("DBContext");
            StaticConfiguration.JWTSecretKey = Configuration["JWTSecretKey"];
            
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StaticConfiguration.JWTSecretKey));
            IdentityModelEventSource.ShowPII = true;



            services.AddAutofac();

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(
                options =>
            {
                options.LoginPath = "/Admin/Account/Login";
                options.Cookie.Name = "KonOgren";
                //options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                options.Cookie.HttpOnly = true;
               
            }
            )
          .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = "KonOgren.com",
                        ValidAudience = "KonOgren.com",
                        IssuerSigningKey = symmetricSecurityKey,
                        RequireExpirationTime = false
                    };
                }); ;

            #region Api Versioning
            // Add API Versioning to the Project
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
            #endregion

            services.AddAutoMapper(typeof(Mappings).GetTypeInfo().Assembly);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpContextAccessor();

            /*services.AddDbContext<KonOgrenDBContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("DBContext")));*/
            services.AddDbContext<KonOgrenDBContext>(o => o.UseSqlite(Configuration.GetConnectionString("DBContext")));
            services.AddMvc();
            services.AddSession();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddRazorPages().AddRazorRuntimeCompilation();

        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assemblyNames = new List<string> { "KonOgren.Domain", "KonOgren.UI", "KonOgren.Services", };
            var assemblies = assemblyNames.Select(assemblyName => Assembly.Load(assemblyName)).ToArray();
            builder.RegisterAssemblyTypes(assemblies)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, KonOgrenDBContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseForwardedHeaders();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseForwardedHeaders();
                app.UseHsts();
            }
            app.Use(async (context, next) =>
            {
                context.Request.Scheme = "https";
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Home/Error/404";
                    await next();
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCookiePolicy();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            StaticConfiguration.wwwrootPath = env.WebRootPath;
            StaticConfiguration.ContentRootPath = env.ContentRootPath;

            SeedDB.Initialize(dbContext);
        }
    }
}
