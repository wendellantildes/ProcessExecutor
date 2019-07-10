using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessExecutor.Domain.Processes;

namespace ProcessExecutor.Infra.Repositories.Config
{
    public class ProcessConfiguration : IEntityTypeConfiguration<Process>
    {
        public void Configure(EntityTypeBuilder<Process> builder)
        {
            builder.ToTable("Process");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.Status).HasColumnName("StatusId");
            builder.HasMany(x => x.Tasks).WithOne().HasForeignKey(x => x.ProcessId);
            builder.HasMany(x => x.Variables).WithOne().HasForeignKey(x => x.ProcessId);
        }
    }
}
