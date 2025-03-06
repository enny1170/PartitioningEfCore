using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Logging;
using PartitioningEfCore.EFCoreMigrationExtensions;



namespace PartitioningEfCore.EfCoreMigrationExtensions
{
    public class MigrationsSqlGeneratorEx : SqlServerMigrationsSqlGenerator
    {
        private readonly IRelationalAnnotationProvider _relationalAnnotationProvider;
            private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
    {
        builder
            .AddConsole()  // 📌 Log-Ausgabe in die Konsole
            .SetMinimumLevel(LogLevel.Information); // Log-Level setzen
    });
        private readonly ILogger<MigrationsSqlGeneratorEx> _logger;
        public MigrationsSqlGeneratorEx(
            MigrationsSqlGeneratorDependencies dependencies,
            ICommandBatchPreparer commandBatchPreparer, IRelationalAnnotationProvider relationalAnnotationProvider)
            : base(dependencies, commandBatchPreparer)
        {
            _relationalAnnotationProvider = relationalAnnotationProvider;
            _logger=_loggerFactory.CreateLogger<MigrationsSqlGeneratorEx>();
        }

        protected override void Generate(MigrationOperation operation, IModel? model, MigrationCommandListBuilder builder)
        {
            if (operation is CreatePartitionFunctionOperation<int> createPartitionFunctionOperationInt)
            {
                Generate(createPartitionFunctionOperationInt, builder);
            }
            else if (operation is CreatePartitionFunctionOperation<short> createPartitionFunctionOperationShort)
            {
                Generate(createPartitionFunctionOperationShort, builder);
            }
            else if (operation is DropPartitionFunctionOperation dropPartitionFunctionOperation)
            {
                Generate(dropPartitionFunctionOperation, builder);
            }
            else if (operation is DropPartitionSchemeOperation dropPartitionSchemeOperation)
            {
                Generate(dropPartitionSchemeOperation, builder);
            }
            else if (operation is CreatePartitionSchemeOperation createPartitionSchemeOperation)
            {
                Generate(createPartitionSchemeOperation, builder);
            }
            else if (operation is CreateTableOperation createTableOperation)
            {
                Generate(createTableOperation, builder, model);
            }
            else
            {
                base.Generate(operation, model, builder);
            }
        }

        private void Generate(CreatePartitionFunctionOperation<int> operation, MigrationCommandListBuilder builder)
        {
            var sqlHelper = Dependencies.SqlGenerationHelper;

            builder.Append("CREATE PARTITION FUNCTION ")
                .Append(sqlHelper.DelimitIdentifier(operation.Name))
                .Append(" (int) AS Range Right for Values ( ")
                .Append(string.Join(",", operation.RangeValues))
                .Append(")")
                .AppendLine(sqlHelper.StatementTerminator)
                .EndCommand();
        }

        private void Generate(CreatePartitionFunctionOperation<short> operation, MigrationCommandListBuilder builder)
        {
            var sqlHelper = Dependencies.SqlGenerationHelper;

            builder.Append("CREATE PARTITION FUNCTION ")
                .Append(sqlHelper.DelimitIdentifier(operation.Name))
                .Append(" (smallint) AS Range Right for Values ( ")
                .Append(string.Join(",", operation.RangeValues))
                .Append(")")
                .AppendLine(sqlHelper.StatementTerminator)
                .EndCommand();
        }

        private void Generate(DropPartitionFunctionOperation operation, MigrationCommandListBuilder builder)
        {
            var sqlHelper = Dependencies.SqlGenerationHelper;

            builder.Append("DROP PARTITION FUNCTION ")
                .Append(sqlHelper.DelimitIdentifier(operation.Name))
                .AppendLine(sqlHelper.StatementTerminator)
                .EndCommand();
        }

        private void Generate(DropPartitionSchemeOperation operation, MigrationCommandListBuilder builder)
        {
            var sqlHelper = Dependencies.SqlGenerationHelper;

            builder.Append("DROP PARTITION Scheme ")
                .Append(sqlHelper.DelimitIdentifier(operation.Name))
                .AppendLine(sqlHelper.StatementTerminator)
                .EndCommand();
        }

        private void Generate(CreatePartitionSchemeOperation operation, MigrationCommandListBuilder builder)
        {
            var sqlHelper = Dependencies.SqlGenerationHelper;

            builder.Append("Create Partition Scheme ")
                .Append(sqlHelper.DelimitIdentifier(operation.Name))
                .Append(" AS PARTITION ")
                .Append(sqlHelper.DelimitIdentifier(operation.FunctionName))
                .Append(" all to ('Primary')")
                .AppendLine(sqlHelper.StatementTerminator)
                .EndCommand();
        }

        private void Generate(CreateTableOperation operation, MigrationCommandListBuilder builder, IModel? model)
        {
            var sqlHelper = Dependencies.SqlGenerationHelper;

            builder.Append("CREATE TABLE ")
                .Append(sqlHelper.DelimitIdentifier(operation.Name))
                .AppendLine(" (");

            for (int i = 0; i < operation.Columns.Count; i++)
            {
                var column = operation.Columns[i];
                builder.Append("    " + sqlHelper.DelimitIdentifier(column.Name))
                    .Append(" ")
                    .Append(column.ColumnType);

                if (column.IsNullable)
                {
                    builder.Append(" NULL");
                }
                else
                {
                    builder.Append(" NOT NULL");
                }

                if (i < operation.Columns.Count - 1)
                {
                    builder.AppendLine(", ");
                }
            }

            // Primärschlüssel hinzufügen
            if (operation.PrimaryKey != null)
            {
                builder.Append(",\n    PRIMARY KEY (")
                    .Append(string.Join(", ", operation.PrimaryKey.Columns.Select(c => sqlHelper.DelimitIdentifier(c))))
                    .AppendLine(")");
            }

            // Einzigartige Constraints hinzufügen
            foreach (var uniqueConstraint in operation.UniqueConstraints)
            {
                builder.Append(", UNIQUE (")
                    .Append(string.Join(", ", uniqueConstraint.Columns.Select(c => sqlHelper.DelimitIdentifier(c))))
                    .AppendLine(")");
            }

            // Fremdschlüssel hinzufügen
            foreach (var foreignKey in operation.ForeignKeys)
            {
                builder.Append(", FOREIGN KEY (")
                    .Append(string.Join(", ", foreignKey.Columns.Select(c => sqlHelper.DelimitIdentifier(c))))
                    .Append(") REFERENCES ")
                    .Append(sqlHelper.DelimitIdentifier(foreignKey.PrincipalTable))
                    .Append(" (")
                    .Append(string.Join(", ", foreignKey.PrincipalColumns.Select(c => sqlHelper.DelimitIdentifier(c))))
                    .Append(")");

                if (foreignKey.OnDelete != ReferentialAction.NoAction)
                {
                    builder.Append(" ON DELETE ")
                        .Append(foreignKey.OnDelete.ToString().ToUpper());
                }
            }
            builder.Append(")");

            // Überprüfen Sie auf Partition-Annotation und generieren Sie zusätzlichen SQL-Code
            _logger.LogInformation("Checking for Partition Annotation");
            _logger.LogInformation("Operation Name: " + operation.Name);
            var entityType = model?.GetEntityTypes().FirstOrDefault(x => x.Name.Split('.').Last() == operation.Name);
            if (entityType != null)
            {
                _logger.LogInformation($"Search at Type {entityType.Name}");
                var x = entityType.AnnotationsToDebugString();
                _logger.LogInformation(x.ToString());
                var partitionSchema = entityType.FindAnnotation("Partition:SchemaName")?.Value?.ToString();
                var partitionField = entityType.FindAnnotation("Partition:FieldName")?.Value?.ToString();
                if (partitionSchema != null && partitionField != null)
                {
                    _logger.LogInformation("Type Has Partition Annotation");
                    builder.Append(" ON ")
                        .Append(sqlHelper.DelimitIdentifier(partitionSchema))
                        .Append(" ( ")
                        .Append(sqlHelper.DelimitIdentifier(partitionField))
                        .Append(")");
                }
                else
                {
                    _logger.LogInformation("Partition Annotation not found");
                    var clrType = entityType.ClrType;
                    _logger.LogInformation($"ClrType: {clrType.Name} has {clrType.CustomAttributes.Count()} custom attributes.");
                    foreach (var attrib in clrType.CustomAttributes)
                    {
                        _logger.LogInformation($"Attribut: {attrib.AttributeType.Name}");
                    }
                }
            }
            else
            {
                _logger.LogInformation("EntityType not found for: " + operation.Name);
            }
        
            builder
                .AppendLine(sqlHelper.StatementTerminator)
                .EndCommand();

        }
    }
}
