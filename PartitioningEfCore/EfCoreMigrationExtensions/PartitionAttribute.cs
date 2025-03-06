using System;


namespace PartitioningEfCore.EfCoreMigrationExtensions
{
    /// <summary>
    /// this Attribute is used to define the partition schema and the partition property
    /// to use for Tabele Creation in SQL Server
    /// The PartitionSchema and the underlaying Patition Function must be exist in the Database
    /// use <see cref="Operations.CreatePartitionFunction"/> and <seealso cref="Operations.CreatePartitionSchema"/> in Migration before />
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PartitionAttribute : Attribute
    {
        public string PartitionSchemaName { get; }
        public string PartitionPropertyName { get; }

        public PartitionAttribute(string partitionSchemaName, string partitionPropertyName)
        {
            PartitionSchemaName = partitionSchemaName;
            PartitionPropertyName = partitionPropertyName;
        }
    }
}