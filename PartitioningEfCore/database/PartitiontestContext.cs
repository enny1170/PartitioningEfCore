using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using PartitioningEfCore.EfCoreMigrationExtensions;

namespace PartitioningEfCore.database;

public partial class PartitiontestContext : DbContext
{
    public PartitiontestContext()
    {
    }

    public PartitiontestContext(DbContextOptions<PartitiontestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PartTestTable> PartTestTables { get; set; }

    public virtual DbSet<TestTable> TestTables { get; set; }

/// <summary>
/// Setup the Connection and register the custom Migration Services
/// </summary>
/// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        optionsBuilder.UseSqlServer("your connection string")
        .ReplaceService<IMigrationsSqlGenerator, MigrationsSqlGeneratorEx>()
        .ReplaceService<IMigrationsAnnotationProvider, CustomMigrationsAnnotationProvider>();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PartTestTable>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Year }).HasName("PK__PartTest__8A1A4A07B5F1E9F5");

            entity.ToTable("PartTestTable");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.Test)
                .HasMaxLength(10)
                .HasColumnName("test");
        });

        modelBuilder.Entity<TestTable>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Year }).HasName("PK__TestTabl__8A1A4A07B536F814");

            entity.ToTable("TestTable");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.Test)
                .HasMaxLength(10)
                .HasColumnName("test");
        });
        

        // Ensure PartitionAttribute will be written to Migration as Annotation
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var entityClrType = entityType.ClrType;
            if (entityClrType == null) continue;

            // Annotation auf der Klasse auslesen
            var entityAnnotation = entityClrType.GetCustomAttribute<PartitionAttribute>();
            if (entityAnnotation != null)
            {
                entityType.SetAnnotation("Partition:SchemaName", entityAnnotation.PartitionSchemaName);
                entityType.SetAnnotation("Partition:FieldName", entityAnnotation.PartitionPropertyName);
            }

        }

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
