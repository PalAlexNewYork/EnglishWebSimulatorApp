using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnglishWebSimulatorApp.Areas.Identity.Data;
using EnglishWebSimulatorApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnglishWebSimulatorApp.Data
{
    public class EnglishWebSimulatorAppContext : IdentityDbContext<EnglishWebSimulatorAppUser>
    {
        public DbSet<Rezults> Rezults { get; set; }
        public DbSet<LibraryEn> libraryEns { get; set; }
        public EnglishWebSimulatorAppContext(DbContextOptions<EnglishWebSimulatorAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
