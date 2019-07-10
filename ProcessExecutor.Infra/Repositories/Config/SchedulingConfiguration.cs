using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessExecutor.Domain;
using ProcessExecutor.Domain.Processes;

namespace ProcessExecutor.Infra.Repositories.Config
{
    public class SchedulingConfiguration : IEntityTypeConfiguration<Scheduling>
    {
        public void Configure(EntityTypeBuilder<Scheduling> builder)
        {
            builder.ToTable("Scheduling");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.Date).HasColumnName("Date");
            builder.Property(x => x.Finished).HasColumnName("Finished");
        }
    }
}
