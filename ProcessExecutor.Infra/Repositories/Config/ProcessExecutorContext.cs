using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using ProcessExecutor.Domain.Processes;

namespace ProcessExecutor.Infra.Repositories.Config
{
    public class ProcessExecutorContext : DbContext
    {
        public ProcessExecutorContext(DbContextOptions options) : base(options)
        {

        }

        public ProcessExecutorContext() : base()
        {

        }

        public DbSet<Process> Processes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlite("Data Source=database.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProcessConfiguration());
            base.OnModelCreating(modelBuilder);

        }
    }
}
