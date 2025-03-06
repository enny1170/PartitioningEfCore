Server=tcp:ai-services.database.windows.net,1433;Initial Catalog=partitiontest;Persist Security Info=False;User ID=sql-admin;Password=schnupper-1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
dotnet ef dbcontext scaffold .-o database "Server=tcp:ai-services.database.windows.net,1433;Initial Catalog=partitiontest;Persist Security Info=False;User ID=sql-admin;Password=schnupper-1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" Microsoft.EntityFrameworkCore.SqlServer
dotnet ef migrations script

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [PartTestTable] (
    [id] uniqueidentifier NOT NULL,
    [year] smallint NOT NULL,
    [test] nvarchar(10) NULL,
    CONSTRAINT [PK__PartTest__8A1A4A07B5F1E9F5] PRIMARY KEY ([id], [year])
);
GO

CREATE TABLE [TestTable] (
    [id] uniqueidentifier NOT NULL,
    [year] smallint NOT NULL,
    [test] nvarchar(10) NULL,
    CONSTRAINT [PK__TestTabl__8A1A4A07B536F814] PRIMARY KEY ([id], [year])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250305124712_first_after_import', N'8.0.1');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [TestTable] ADD [Test2] nvarchar(max) NULL;
GO

ALTER TABLE [PartTestTable] ADD [Test2] nvarchar(max) NULL;
GO

ALTER TABLE [PartTestTable] ADD [Test3] int NULL;
GO

CREATE PARTITION FUNCTION [TestPartFunction] (smallint) AS Range Right for Values ( 1980,1982,1984);
GO

CREATE PARTITION FUNCTION [TestIntPartition] (int) AS Range Right for Values ( 22,23,24,25,26,27,28);
GO

DROP PARTITION FUNCTION [TestIntPartition];
GO

Create Partition Scheme [TestSchemeA] AS PARTITION [TestPartFunction] all to ('Primary');
GO

DROP PARTITION Scheme [TestSchemeA];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250305124918_add_test_fields', N'8.0.1');
GO

COMMIT;
GO