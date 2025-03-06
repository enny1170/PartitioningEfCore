using System;
using PartitioningEfCore.EfCoreMigrationExtensions;

namespace PartitioningEfCore.database;
[Partition("TestMertSheme", "Year")]
public partial class TestYear
{
    public Guid Id { get; set; }

    public short Year { get; set; }

    public string? Testfeld { get; set; }
}
