IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170622162223_RemoveGRMEventIdNullability')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'Service.BaseValueSegment.BVSValueHeader') AND [c].[name] = N'GRMEventId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Service.BaseValueSegment].[BVSValueHeader] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Service.BaseValueSegment].[BVSValueHeader] ALTER COLUMN [GRMEventId] int NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170622162223_RemoveGRMEventIdNullability')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'Service.BaseValueSegment.BVSOwner') AND [c].[name] = N'GRMEventId');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Service.BaseValueSegment].[BVSOwner] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Service.BaseValueSegment].[BVSOwner] ALTER COLUMN [GRMEventId] int NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170622162223_RemoveGRMEventIdNullability')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170622162223_RemoveGRMEventIdNullability', N'1.1.1');
END;

GO

