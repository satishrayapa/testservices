IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170802195455_AddColumnAlphaBVSOwnerId')
BEGIN
    ALTER TABLE [Service.BaseValueSegment].[BVSOwner] ADD [AlphaBVSOwnerId] int
	CONSTRAINT AlphaBVSOwnerId_ID_FK FOREIGN KEY (AlphaBVSOwnerId)
    REFERENCES [Service.BaseValueSegment].[BVSOwner](Id)
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170802195455_AddColumnAlphaBVSOwnerId')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170802195455_AddColumnAlphaBVSOwnerId', N'1.1.1');
END;

GO


