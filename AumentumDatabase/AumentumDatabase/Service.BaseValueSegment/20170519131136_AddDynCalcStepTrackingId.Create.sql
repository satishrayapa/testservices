IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170519131136_AddDynCalcStepTrackingId')
BEGIN
    ALTER TABLE [Service.BaseValueSegment].[BVSValueHeader] ADD [DynCalcStepTrackingId] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170519131136_AddDynCalcStepTrackingId')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'Service.BaseValueSegment.BVSValue') AND [c].[name] = N'DynCalcStepTrackingId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Service.BaseValueSegment].[BVSValue] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Service.BaseValueSegment].[BVSValue] ALTER COLUMN [DynCalcStepTrackingId] int NOT NULL;
    ALTER TABLE [Service.BaseValueSegment].[BVSValue] ADD DEFAULT 0 FOR [DynCalcStepTrackingId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170519131136_AddDynCalcStepTrackingId')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'Service.BaseValueSegment.BVSOwnerValue') AND [c].[name] = N'DynCalcStepTrackingId');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Service.BaseValueSegment].[BVSOwnerValue] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Service.BaseValueSegment].[BVSOwnerValue] ALTER COLUMN [DynCalcStepTrackingId] int NOT NULL;
    ALTER TABLE [Service.BaseValueSegment].[BVSOwnerValue] ADD DEFAULT 0 FOR [DynCalcStepTrackingId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170519131136_AddDynCalcStepTrackingId')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'Service.BaseValueSegment.BVSOwner') AND [c].[name] = N'DynCalcStepTrackingId');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Service.BaseValueSegment].[BVSOwner] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Service.BaseValueSegment].[BVSOwner] ALTER COLUMN [DynCalcStepTrackingId] int NOT NULL;
    ALTER TABLE [Service.BaseValueSegment].[BVSOwner] ADD DEFAULT 0 FOR [DynCalcStepTrackingId];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170519131136_AddDynCalcStepTrackingId')
BEGIN
    ALTER TABLE [Service.BaseValueSegment].[BVS] ADD [DynCalcStepTrackingId] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170519131136_AddDynCalcStepTrackingId')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170519131136_AddDynCalcStepTrackingId', N'1.1.1');
END;

GO

