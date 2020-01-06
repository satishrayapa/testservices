IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170406201955_Initial')
BEGIN
    DROP TABLE [Common.Resource].[Resource];
END;

GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170406201955_Initial')
BEGIN
    DELETE FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20170406201955_Initial';
END;

GO

