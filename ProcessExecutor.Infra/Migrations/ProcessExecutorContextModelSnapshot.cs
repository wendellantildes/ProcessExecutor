﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProcessExecutor.Infra.Repositories.Config;

namespace ProcessExecutor.Infra.Migrations
{
    [DbContext(typeof(ProcessExecutorContext))]
    partial class ProcessExecutorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("ProcessExecutor.Domain.Processes.Process", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<int>("Status")
                        .HasColumnName("StatusId");

                    b.HasKey("Id");

                    b.ToTable("Process");
                });

            modelBuilder.Entity("ProcessExecutor.Domain.Processes.Task", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<Guid>("ProcessId")
                        .HasColumnName("ProcessId");

                    b.Property<int>("Status")
                        .HasColumnName("StatusId");

                    b.Property<int>("Step")
                        .HasColumnName("StepId");

                    b.Property<int>("System")
                        .HasColumnName("SystemId");

                    b.HasKey("Id");

                    b.HasIndex("ProcessId");

                    b.ToTable("Task");
                });

            modelBuilder.Entity("ProcessExecutor.Domain.Processes.Variable", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .HasColumnName("Name");

                    b.Property<Guid?>("ProcessId")
                        .HasColumnName("ProcessId");

                    b.Property<Guid?>("TaskId")
                        .HasColumnName("TaskId");

                    b.Property<int>("Type")
                        .HasColumnName("Type");

                    b.Property<string>("Value")
                        .HasColumnName("Value");

                    b.HasKey("Id");

                    b.HasIndex("ProcessId");

                    b.HasIndex("TaskId");

                    b.ToTable("Variable");
                });

            modelBuilder.Entity("ProcessExecutor.Domain.Scheduling", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTime>("Date")
                        .HasColumnName("Date");

                    b.Property<bool>("Finished")
                        .HasColumnName("Finished");

                    b.HasKey("Id");

                    b.ToTable("Scheduling");
                });

            modelBuilder.Entity("ProcessExecutor.Domain.Processes.Task", b =>
                {
                    b.HasOne("ProcessExecutor.Domain.Processes.Process")
                        .WithMany("Tasks")
                        .HasForeignKey("ProcessId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProcessExecutor.Domain.Processes.Variable", b =>
                {
                    b.HasOne("ProcessExecutor.Domain.Processes.Process")
                        .WithMany("Variables")
                        .HasForeignKey("ProcessId");

                    b.HasOne("ProcessExecutor.Domain.Processes.Task")
                        .WithMany("Variables")
                        .HasForeignKey("TaskId");
                });
#pragma warning restore 612, 618
        }
    }
}
