using DashBe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Infrastructure.Configuration
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Logs");

            builder.HasKey(log => log.Id);

            builder.Property(log => log.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(log => log.Message)
                   .IsRequired()
                   .HasColumnType("TEXT");

            builder.Property(log => log.LogLevel)
                   .IsRequired()
                   .HasColumnType("TEXT");

            builder.Property(log => log.Exception)
                   .HasColumnType("TEXT");

            builder.Property(log => log.Timestamp)
                   .IsRequired()
                   .HasColumnType("DATETIME");
        }
    }
}
