using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Wifi> Wifis { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<UserDevice> UsersDevices { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Measurement>().Property(p => p.IsRead).HasDefaultValue(false);
            //modelBuilder.Entity<Measurement>().Property(p => p.UserId).HasDefaultValue(null);
            //modelBuilder.Entity<Measurement>().HasOne(m => m.User).WithMany(u => u.Measurements).HasForeignKey(m => m.UserId).IsRequired(false);
        }
    }
}
