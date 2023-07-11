using EnglishWebSimulatorApp.Areas.Identity.Data;
using EnglishWebSimulatorApp.Data;
using EnglishWebSimulatorApp.Models.Interfaces;
using EnglishWebSimulatorApp.Models.Repository;
using EnglishWebSimulatorApp.Models.Servise;
using EnglishWebSimulatorApp.Models.WorkJson;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace EnglishWebSimulatorApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddAuthentication();
            services.AddAuthorization();
            services.AddControllersWithViews();
            services.AddMvc((options) => options.EnableEndpointRouting = false)
                .WithRazorPagesRoot("/Areas");
            services.AddScoped<ILibraryServise, LibraryServise>();
            services.AddSingleton<IlibraryEnShServise, LibraryEnShService>();
            services.AddSingleton<ILibraryWorkJson, LibraryWorkJson>();
            services.AddSingleton<ILibraryWorkJsonServise, LibraryWorkJsonServise>();
            services.AddControllersWithViews();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };

               //options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

          
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        { 
            
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDeveloperExceptionPage();
            app.UseRequestLocalization();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            
        }
    }
}
