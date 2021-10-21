using DevOps.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevOps.Infrastructure
{
    class DeveloperConfiguration : IEntityTypeConfiguration<Developer>
    {
        public void Configure(EntityTypeBuilder<Developer> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).HasMaxLength(11);
            builder.Property(d => d.FirstName).IsRequired();
            builder.Property(d => d.LastName).IsRequired();
            builder.Property(d => d.Rating).HasConversion(p => new Percentage(p), p => p);

        }
    }
}
