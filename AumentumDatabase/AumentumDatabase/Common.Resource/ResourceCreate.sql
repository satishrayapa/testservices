IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170406201955_Initial')
BEGIN
    IF SCHEMA_ID(N'Common.Resource') IS NULL EXEC(N'CREATE SCHEMA [Common.Resource];');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170406201955_Initial')
BEGIN
    CREATE TABLE [Common.Resource].[Resource] (
        [Key] nvarchar(200) NOT NULL,
        [Partition] nvarchar(200) NOT NULL,
        [Value] nvarchar(1000),
        CONSTRAINT [PK_Resource] PRIMARY KEY ([Key], [Partition])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170406201955_Initial')
BEGIN
    INSERT INTO [Common.Resource].[Resource] VALUES('facade.assessmentheader','urlservices:http://localhost:50001','http://localhost:50206');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170406201955_Initial')
BEGIN
    INSERT INTO [Common.Resource].[Resource] VALUES('facade.basevaluesegment','urlservices:http://localhost:50001','http://localhost:50205');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170406201955_Initial')
BEGIN
    INSERT INTO [Common.Resource].[Resource] VALUES('service.assessmentevent','urlservices:http://localhost:50001','http://localhost:50201');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170406201955_Initial')
BEGIN
    INSERT INTO [Common.Resource].[Resource] VALUES('service.legalparty','urlservices:http://localhost:50001','http://localhost:50203');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170406201955_Initial')
BEGIN
    INSERT INTO [Common.Resource].[Resource] VALUES('service.basevaluesgement','urlservices:http://localhost:50001','http://localhost:50204');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170406201955_Initial')
BEGIN
    INSERT INTO [Common.Resource].[Resource] VALUES('service.revenueobject','urlservices:http://localhost:50001','http://localhost:50202');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170406201955_Initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170406201955_Initial', N'1.1.1');
END;

GO

