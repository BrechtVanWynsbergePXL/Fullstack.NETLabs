using DevOps.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevOps.Infrastructure
{
    class DevOpsContext : DbContext
    {
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Team> Teams { get; set; }

        public DevOpsContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Percentage>().HasKey(p => p._value);
            modelBuilder.Entity<Developer>()
                .HasKey(d => d.Id);
            //modelBuilder.Entity<Developer>().HasOne(d => d.Rating);
            modelBuilder.Entity<Team>().HasKey(t => t.Id);
        }
    }
}
