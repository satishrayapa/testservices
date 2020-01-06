IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    DROP TABLE [Service.BaseValueSegment].[AsmtRevnBVS];
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    DROP TABLE [Service.BaseValueSegment].[BVSOwnerValue];
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    DROP TABLE [Service.BaseValueSegment].[BVSValue];
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    DROP TABLE [Service.BaseValueSegment].[BVSStatusType];
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    DROP TABLE [Service.BaseValueSegment].[BVSOwner];
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    DROP TABLE [Service.BaseValueSegment].[BVSValueHeader];
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    DROP TABLE [Service.BaseValueSegment].[BVSTran];
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    DROP TABLE [Service.BaseValueSegment].[BVS];
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    DROP TABLE [Service.BaseValueSegment].[BVSTranType];
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    DELETE FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20170425185408_InitialCreate';
END;

GO

