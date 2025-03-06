using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using PartitioningEfCore.EFCoreMigrationExtensions;

namespace MigrationsBuilder;

/// <summary>
/// Registering then added Operation in Operationsbuilder
/// https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/operations
/// </summary>
public static partial class Operations
{
    /// <summary>
    /// Create a Partition Function
    /// </summary>
    /// <typeparam name="Trange"></typeparam>
    /// <param name="migrationBuilder"></param>
    /// <param name="name"></param>
    /// <param name="rangeValues"></param>
    /// <returns></returns>
    public static OperationBuilder<CreatePartitionFunctionOperation<Trange>> CreatePartitionFunction<Trange>(
        this MigrationBuilder migrationBuilder, string name, List<Trange> rangeValues)
    {
        var operation = new CreatePartitionFunctionOperation<Trange> { Name = name, RangeValues = rangeValues };
        migrationBuilder.Operations.Add(operation);

        return new OperationBuilder<CreatePartitionFunctionOperation<Trange>>(operation);
    }

/// <summary>
/// Drop a Partition Function
/// </summary>
/// <param name="migrationBuilder"></param>
/// <param name="name"></param>
/// <returns></returns>
    public static OperationBuilder<DropPartitionFunctionOperation> DropPartitionFunction(
        this MigrationBuilder migrationBuilder, string name)
    {
        var operation = new DropPartitionFunctionOperation { Name = name };
        migrationBuilder.Operations.Add(operation);

        return new OperationBuilder<DropPartitionFunctionOperation>(operation);
    }

/// <summary>
/// Drop a Partition Scheme
/// </summary>
/// <param name="migrationBuilder"></param>
/// <param name="name"></param>
/// <returns></returns>
    public static OperationBuilder<DropPartitionSchemeOperation> DropPartitionScheme(
        this MigrationBuilder migrationBuilder, string name)
    {
        var operation = new DropPartitionSchemeOperation { Name = name };
        migrationBuilder.Operations.Add(operation);

        return new OperationBuilder<DropPartitionSchemeOperation>(operation);
    }

/// <summary>
/// Create a Partition Scheme based on a Partition Function
/// </summary>
/// <param name="migrationBuilder"></param>
/// <param name="name">name of the Scheme</param>
/// <param name="functionName">Name of a existing PartitionFunction</param>
/// <returns></returns>
    public static OperationBuilder<CreatePartitionSchemeOperation> CreatePartitionScheme(
        this MigrationBuilder migrationBuilder, string name, string functionName)
    {
        var operation = new CreatePartitionSchemeOperation { Name = name, FunctionName = functionName };
        migrationBuilder.Operations.Add(operation);

        return new OperationBuilder<CreatePartitionSchemeOperation>(operation);
    }

}
