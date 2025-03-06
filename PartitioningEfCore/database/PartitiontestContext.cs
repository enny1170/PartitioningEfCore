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

    private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
    {
        builder
            .AddConsole()  // 📌 Log-Ausgabe in die Konsole
            .SetMinimumLevel(LogLevel.Information); // Log-Level setzen
    });

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // var loggerFactory = LoggerFactory.Create(builder =>
        // {
        //     builder
        //         .AddConsole()
        //         .SetMinimumLevel(LogLevel.Debug);
        // });
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        optionsBuilder.UseSqlServer("Server=tcp:ai-services.database.windows.net,1433;Initial Catalog=partitiontest;Persist Security Info=False;User ID=sql-admin;Password=schnupper-1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
        .ReplaceService<IMigrationsSqlGenerator, MigrationsSqlGeneratorEx>()
        .ReplaceService<IMigrationsAnnotationProvider,CustomMigrationsAnnotationProvider>()
        .UseLoggerFactory(_loggerFactory)
        .EnableSensitiveDataLogging();
        //.LogTo(Console.WriteLine, LogLevel.Debug);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ILogger<PartitiontestContext> _logger= _loggerFactory.CreateLogger<PartitiontestContext>();
        _logger.LogInformation("OnModelCreating");        
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
        _logger.LogInformation("Check for Entity Attributes");
        // Custom configurations can be added here
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var entityClrType = entityType.ClrType;
            if (entityClrType == null) continue;

            // Annotation auf der Klasse auslesen
            var entityAnnotation = entityClrType.GetCustomAttribute<PartitionAttribute>();
            if (entityAnnotation != null)
            {
                _logger.LogInformation("Attribut gefunden an Typ {0}", entityClrType.Name);
                entityType.SetAnnotation("Partition:SchemaName", entityAnnotation.PartitionSchemaName);
                _logger.LogInformation("Set Partition:SchemaName: {0}", entityAnnotation.PartitionSchemaName);
                entityType.SetAnnotation("Partition:FieldName", entityAnnotation.PartitionPropertyName);
                _logger.LogInformation("Set Partition:FieldName: {0}", entityAnnotation.PartitionPropertyName);
                var a1 = entityType.FindAnnotation("Partition:SchemaName")?.Value; // "TestSchemeA"
                var a2 = entityType.FindAnnotation("Partition:FieldName")?.Value; // "Year"
                _logger.LogInformation("Partition:SchemaName: {0}", a1);
                _logger.LogInformation("Partition:FieldName: {0}", a2);
            }
            else
            {
                _logger.LogInformation("Attribut nicht gefunden an Typ {0}", entityClrType.Name);
            }

            // Annotationen für Properties auslesen
            // foreach (var property in entityClrType.GetProperties())
            // {
            //     var propertyAnnotation = property.GetCustomAttribute<CustomAnnotationAttribute>();
            //     if (propertyAnnotation != null)
            //     {
            //         var propertyMetadata = entityType.FindProperty(property.Name);
            //         if (propertyMetadata != null)
            //         {
            //             propertyMetadata.SetAnnotation("Custom:PropertyInfo", propertyAnnotation.Value);
            //         }
            //     }
            // }
        }

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
