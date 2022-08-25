using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DutchTreat
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<StoreUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<DutchContext>();

            services.AddDbContext<DutchContext>();

            services.AddTransient<IMailService, NullMailService>();
            services.AddTransient<DutchSeeder>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IDutchRepository, DutchRepository>();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(cfg =>
                {
                    cfg.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(cfg =>
            {
                cfg.MapRazorPages();

                cfg.MapControllerRoute("Default",
                    "/{controller}/{action}/{id?}",
                    new { controller = "App", action = "Index" });
            });
        }
    }
}
