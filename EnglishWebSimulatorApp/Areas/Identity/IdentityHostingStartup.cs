using System;
using EnglishWebSimulatorApp.Areas.Identity.Data;
using EnglishWebSimulatorApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(EnglishWebSimulatorApp.Areas.Identity.IdentityHostingStartup))]
namespace EnglishWebSimulatorApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<EnglishWebSimulatorAppContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("EnglishWebSimulatorAppContextConnection")));

                services.AddDefaultIdentity<EnglishWebSimulatorAppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<EnglishWebSimulatorAppContext>();
            });
        }
    }
}