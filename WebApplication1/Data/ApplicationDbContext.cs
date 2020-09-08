using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext :  IdentityDbContext<ApplicationUser>
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           //Database.EnsureDeleted();
           Database.EnsureCreated();
              // удаляем бд со старой схемой
            //Database.EnsureCreated();   // создаем бд с новой схемой
        }
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    // Customize the ASP.NET Identity model and override the defaults if needed.
        //    // For example, you can rename the ASP.NET Identity table names and more.
        //    // Add your customizations after calling base.OnModelCreating(builder);
        //}
        public DbSet<Conference> Conferences { get; set; }
        
        public DbSet<CalendarEvent> CalendarEvents { get; set; }
        public DbSet<UserGoConf> UserGoConfs { get; set; }
        public DbSet<MyFile> MyFiles { get; set; }
    }
}
