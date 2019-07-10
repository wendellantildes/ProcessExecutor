using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessExecutor.Domain.Processes;

namespace ProcessExecutor.Infra.Repositories.Config
{
    public class VariableConfiguration : IEntityTypeConfiguration<Variable>
    {
        public void Configure(EntityTypeBuilder<Variable> builder)
        {
            builder.ToTable("Variable");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.Type).HasColumnName("Type");
            builder.Property(x => x.Value).HasColumnName("Value");
            builder.Property(x => x.TaskId).HasColumnName("TaskId");
            builder.Property(x => x.ProcessId).HasColumnName("ProcessId");
        }
    }
}
