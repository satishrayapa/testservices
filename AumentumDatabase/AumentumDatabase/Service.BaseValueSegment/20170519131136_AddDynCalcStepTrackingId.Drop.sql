IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170519131136_AddDynCalcStepTrackingId')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'Service.BaseValueSegment.BVSValueHeader') AND [c].[name] = N'DynCalcStepTrackingId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Service.BaseValueSegment].[BVSValueHeader] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Service.BaseValueSegment].[BVSValueHeader] DROP COLUMN [DynCalcStepTrackingId];
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170519131136_AddDynCalcStepTrackingId')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'Service.BaseValueSegment.BVS') AND [c].[name] = N'DynCalcStepTrackingId');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Service.BaseValueSegment].[BVS] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Service.BaseValueSegment].[BVS] DROP COLUMN [DynCalcStepTrackingId];
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170519131136_AddDynCalcStepTrackingId')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'Service.BaseValueSegment.BVSValue') AND [c].[name] = N'DynCalcStepTrackingId');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Service.BaseValueSegment].[BVSValue] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Service.BaseValueSegment].[BVSValue] ALTER COLUMN [DynCalcStepTrackingId] int NOT NULL;
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170519131136_AddDynCalcStepTrackingId')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'Service.BaseValueSegment.BVSOwnerValue') AND [c].[name] = N'DynCalcStepTrackingId');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Service.BaseValueSegment].[BVSOwnerValue] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [Service.BaseValueSegment].[BVSOwnerValue] ALTER COLUMN [DynCalcStepTrackingId] int NOT NULL;
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170519131136_AddDynCalcStepTrackingId')
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'Service.BaseValueSegment.BVSOwner') AND [c].[name] = N'DynCalcStepTrackingId');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Service.BaseValueSegment].[BVSOwner] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [Service.BaseValueSegment].[BVSOwner] ALTER COLUMN [DynCalcStepTrackingId] int NOT NULL;
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170519131136_AddDynCalcStepTrackingId')
BEGIN
    DELETE FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20170519131136_AddDynCalcStepTrackingId';
END;

GO

