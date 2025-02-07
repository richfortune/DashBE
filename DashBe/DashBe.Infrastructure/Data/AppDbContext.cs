using DashBe.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<RoleUser> RoleUser { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<RoleUser>()
        //        .HasKey(ur => new { ur.UserId, ur.RoleId }); // 🔥 Definiamo la chiave primaria composta

        //    modelBuilder.Entity<RoleUser>()
        //        .HasOne(ur => ur.User)
        //        .WithMany(u => u.RoleUsers)
        //        .HasForeignKey(ur => ur.UserId);

        //    modelBuilder.Entity<RoleUser>()
        //        .HasOne(ur => ur.Role)
        //        .WithMany(r => r.RoleUsers)
        //        .HasForeignKey(ur => ur.RoleId);
        //}





    }
}
