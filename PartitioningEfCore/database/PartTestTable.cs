using System;
using System.Collections.Generic;
using PartitioningEfCore.EfCoreMigrationExtensions;

namespace PartitioningEfCore.database;
[Partition("TestSchemeA", "Year")]
public partial class PartTestTable
{
    public Guid Id { get; set; }

    public short Year { get; set; }

    public string? Test { get; set; }

    public string? Test2 { get; set; }
    public int? Test3 { get; set; }
}