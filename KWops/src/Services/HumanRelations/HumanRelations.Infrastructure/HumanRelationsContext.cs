using HumanRelations.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HumanRelations.Infrastructure
{
    public class HumanRelationsContext : DbContext
    {
        public DbSet<IEmployee> Employee { get; set; }

        public HumanRelationsContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IEmployee>()
                .HasKey(employee => employee.Number);
        }
    }
}
