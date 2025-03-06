using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace PartitioningEfCore.EFCoreMigrationExtensions
{
    /// <summary>
    /// Represents a CreatePartitionFunctions Migration Operation 
    /// </summary>
    public class CreatePartitionFunctionOperation<Trange> : MigrationOperation
    {
        public string Name { get; set; } = "";
        public List<Trange> RangeValues { get; set; } = new List<Trange>();
    }

    /// <summary>
    /// Represents a CreatePartitionFunction Migration Operation 
    /// </summary>
    public class DropPartitionFunctionOperation : MigrationOperation
    {
        public string Name { get; set; } = "";
    }

    /// <summary>
    /// Represents a CreatePartitionSheme Migration Operation 
    /// </summary>
    public class CreatePartitionSchemeOperation : MigrationOperation
    {
        public string Name { get; set; } = "";
        /// <summary>
        /// This means the name of the PartionFunction to use, and must be allready created
        /// </summary>
        public string FunctionName { get; set; } = "";
    }

    /// <summary>
    /// Represents a DropPartitionFunction Migration Operation 
    /// </summary>
    public class DropPartitionSchemeOperation : MigrationOperation
    {
        public string Name { get; set; } = "";
    }

    public class PartitionCreateTableOperation : CreateTableOperation
    {
        public string? PartitionSchemaName { get; set; } 
        public string? PartitionSchemaProperty { get; set; } 
    }

    // public class PartitionCreateTableBuilder : CreateTableBuilder<PartitionCreateTableOperation>
    // {
    //     public PartitionCreateTableBuilder(PartitionCreateTableOperation operation, IReadOnlyDictionary<PropertyInfo, AddColumnOperation> columnMap)
    //     : base(operation, columnMap)
    //     { }

    //     public PartitionCreateTableBuilder WithPartitionParameter(string? partitionName,string ?partitionField)
    //     {
    //         var op = Operation as PartitionCreateTableOperation;
    //         if (op != null)
    //         {
    //             op.PartitionSchemaName = partitionName;
    //             op.PartitionSchemaProperty = partitionField;
    //         }
    //         return this;
    //     }
    // }
}