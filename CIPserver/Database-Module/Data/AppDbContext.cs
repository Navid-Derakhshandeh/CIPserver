using DataBaseModule.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModule.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ConnectionConfig> connectionConfigs => Set<ConnectionConfig>();
        public DbSet<User> Users => Set<User>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Username)
                .IsUnique();
        }
    }
}
