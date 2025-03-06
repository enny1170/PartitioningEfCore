// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using PartitioningEfCore.database;
using PartitioningEfCore.EfCoreMigrationExtensions;

Console.WriteLine("Hello, World!");
var context = new PartitiontestContext();
foreach (var entityType in context.Model.GetEntityTypes())
{
    var entityClrType = entityType.ClrType;
    Debug.WriteLine($"Type {entityClrType.Name}");
    if (entityClrType == null) continue;
    var attrib = entityClrType.GetCustomAttributes(typeof(PartitionAttribute), false).FirstOrDefault();
    if (attrib != null)
    {
        Console.WriteLine("Attribute found");
    }
    else
    {
        Console.WriteLine("Attribute not found");
    }
}
