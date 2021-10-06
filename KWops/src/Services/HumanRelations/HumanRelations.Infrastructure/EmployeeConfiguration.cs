using HumanRelations.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HumanRelations.Infrastructure
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<IEmployee>
    {
        public void Configure(EntityTypeBuilder<IEmployee> builder)
        {
            builder.HasKey(e => e.Number);
            builder.Property(e => e.Number).HasMaxLength(11).HasConversion(n => n.ToString(), s => new EmployeeNumber(s));
            builder.Property(e => e.LastName).IsRequired();
            builder.Property(e => e.FirstName).IsRequired();
        }
    }
}
