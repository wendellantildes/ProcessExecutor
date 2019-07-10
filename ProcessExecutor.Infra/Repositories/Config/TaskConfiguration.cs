using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessExecutor.Domain.Processes;

namespace ProcessExecutor.Infra.Repositories.Config
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToTable("Task");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.Status).HasColumnName("StatusId");
            builder.Property(x => x.Step).HasColumnName("StepId");
            builder.Property(x => x.System).HasColumnName("SystemId");
            builder.Property(x => x.ProcessId).HasColumnName("ProcessId");
            builder.HasMany(x => x.Variables).WithOne().HasForeignKey(x => x.TaskId);
        }
    }
}
