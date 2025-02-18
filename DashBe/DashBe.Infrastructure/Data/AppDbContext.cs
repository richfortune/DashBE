using DashBe.Application.Interfaces;
using DashBe.Domain.Entities;
using DashBe.Domain.Models;
using DashBe.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Infrastructure.Data
{
    public class AppDbContext : DbContext, IApplicationDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<RoleUser> RoleUser { get; set; }

        public DbSet<Log> Logs { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleUser>()
               .HasKey(ru => ru.Id); // Usa Id come PK

            modelBuilder.Entity<RoleUser>()
                .HasOne(ru => ru.User)
                .WithMany(u => u.RoleUsers)
                .HasForeignKey(ru => ru.UserId);

            modelBuilder.Entity<RoleUser>()
                .HasOne(ru => ru.Role)
                .WithMany(r => r.RoleUsers)
                .HasForeignKey(ru => ru.RoleId);

            modelBuilder.ApplyConfiguration(new LogConfiguration()); // Configura la tabella Logs
        }

        public IQueryable<T> GetData<T>(bool trackingChanges = false) where T : class
        {
            var set = Set<T>();
            return trackingChanges ? set.AsTracking() : set.AsNoTracking();
        }

        public void Insert<T>(T entity) where T : class => Set<T>().Add(entity);
        public void Update<T>(T entity) where T : class => Set<T>().Update(entity);
        public void Delete<T>(T entity) where T : class => Set<T>().Remove(entity);
        public void DeleteRange<T>(IEnumerable<T> entities) where T : class => Set<T>().RemoveRange(entities);


        public Task SaveAsync() => SaveChangesAsync();

        public async Task<bool> ExistsAsync<T>(params object[] key) where T : class
        {
            return await Set<T>().FindAsync(key) != null;
        }

        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : class
        {
            return await Set<T>().AnyAsync(predicate, cancellationToken);
        }

        public async Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : class
        {
            return await Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public DatabaseFacade DatabaseFacade => base.Database;

        public async Task LogAsync(string message, string logLevel, string exception = null)
        {
            var log = new Log
            {
                Message = message,
                LogLevel = logLevel,
                Exception = exception ?? "",
                Timestamp = DateTime.UtcNow
            };

            Logs.Add(log);
            await SaveAsync();
        }

        











    }
}
