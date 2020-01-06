IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    IF SCHEMA_ID(N'Service.BaseValueSegment') IS NULL EXEC(N'CREATE SCHEMA [Service.BaseValueSegment]; AUTHORIZATION [dbo];');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE TABLE [Service.BaseValueSegment].[BVS] (
        [Id] int NOT NULL IDENTITY,
        [AsOf] date NOT NULL,
        [DynCalcInstanceId] int NOT NULL,
        [RevObjId] int NOT NULL,
        [SequenceNumber] int NOT NULL,
        [TranId] bigint NOT NULL,
        CONSTRAINT [PK_BVS] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE TABLE [Service.BaseValueSegment].[BVSStatusType] (
        [Id] int NOT NULL,
        [Description] varchar(100),
        [Name] varchar(50) NOT NULL,
        CONSTRAINT [PK_BVSStatusType] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE TABLE [Service.BaseValueSegment].[BVSTranType] (
        [Id] int NOT NULL,
        [Description] varchar(100) NOT NULL,
        [Name] varchar(50) NOT NULL,
        CONSTRAINT [PK_BVSTranType] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE TABLE [Service.BaseValueSegment].[AsmtRevnBVS] (
        [Id] int NOT NULL IDENTITY,
        [AsmtRevnId] int NOT NULL,
        [BVSId] int,
        [BVSStatusTypeId] int NOT NULL,
        [DynCalcStepTrackingId] int NOT NULL,
        [ReviewMessage] varchar(1024) NOT NULL,
        CONSTRAINT [PK_AsmtRevnBVS] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AsmtRevnBVS_BVS_BVSId] FOREIGN KEY ([BVSId]) REFERENCES [Service.BaseValueSegment].[BVS] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_AsmtRevnBVS_BVSStatusType_BVSStatusTypeId] FOREIGN KEY ([BVSStatusTypeId]) REFERENCES [Service.BaseValueSegment].[BVSStatusType] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE TABLE [Service.BaseValueSegment].[BVSTran] (
        [Id] int NOT NULL IDENTITY,
        [BVSId] int NOT NULL,
        [BVSTranTypeId] int NOT NULL,
        [DynCalcStepTrackingId] int,
        [EffStatus] char(1) NOT NULL,
        [TranId] bigint NOT NULL,
        CONSTRAINT [PK_BVSTran] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BVSTran_BVS_BVSId] FOREIGN KEY ([BVSId]) REFERENCES [Service.BaseValueSegment].[BVS] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_BVSTran_BVSTranType_BVSTranTypeId] FOREIGN KEY ([BVSTranTypeId]) REFERENCES [Service.BaseValueSegment].[BVSTranType] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE TABLE [Service.BaseValueSegment].[BVSOwner] (
        [Id] int NOT NULL IDENTITY,
        [BVSTranId] int NOT NULL,
        [BIPercent] decimal(28, 10) NOT NULL,
        [DynCalcStepTrackingId] int NOT NULL,
        [GRMEventId] int,
        [LegalPartyRoleId] int NOT NULL,
        CONSTRAINT [PK_BVSOwner] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BVSOwner_BVSTran_BVSTranId] FOREIGN KEY ([BVSTranId]) REFERENCES [Service.BaseValueSegment].[BVSTran] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE TABLE [Service.BaseValueSegment].[BVSValueHeader] (
        [Id] int NOT NULL IDENTITY,
        [BVSTranId] int NOT NULL,
        [BaseYear] int NOT NULL,
        [GRMEventId] int,
        CONSTRAINT [PK_BVSValueHeader] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BVSValueHeader_BVSTran_BVSTranId] FOREIGN KEY ([BVSTranId]) REFERENCES [Service.BaseValueSegment].[BVSTran] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE TABLE [Service.BaseValueSegment].[BVSOwnerValue] (
        [Id] int NOT NULL IDENTITY,
        [BaseValue] decimal(28, 10) NOT NULL,
        [BVSOwnerId] int NOT NULL,
        [BVSValueHeaderId] int NOT NULL,
        [DynCalcStepTrackingId] int NOT NULL,
        CONSTRAINT [PK_BVSOwnerValue] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BVSOwnerValue_BVSOwner_BVSOwnerId] FOREIGN KEY ([BVSOwnerId]) REFERENCES [Service.BaseValueSegment].[BVSOwner] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_BVSOwnerValue_BVSValueHeader_BVSValueHeaderId] FOREIGN KEY ([BVSValueHeaderId]) REFERENCES [Service.BaseValueSegment].[BVSValueHeader] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE TABLE [Service.BaseValueSegment].[BVSValue] (
        [Id] int NOT NULL IDENTITY,
        [BVSValueHeaderId] int NOT NULL,
        [DynCalcStepTrackingId] int NOT NULL,
        [FullValueAmount] decimal(28, 10) NOT NULL,
        [PctComplete] decimal(14, 10) NOT NULL,
        [SubComponent] int NOT NULL,
        [ValueAmount] decimal(28, 10) NOT NULL,
        CONSTRAINT [PK_BVSValue] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BVSValue_BVSValueHeader_BVSValueHeaderId] FOREIGN KEY ([BVSValueHeaderId]) REFERENCES [Service.BaseValueSegment].[BVSValueHeader] ([Id]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE INDEX [IX_AsmtRevnBVS_BVSId] ON [Service.BaseValueSegment].[AsmtRevnBVS] ([BVSId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE INDEX [IX_AsmtRevnBVS_BVSStatusTypeId] ON [Service.BaseValueSegment].[AsmtRevnBVS] ([BVSStatusTypeId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE UNIQUE INDEX [IX_BVS_AsOf_RevObjId_SequenceNumber] ON [Service.BaseValueSegment].[BVS] ([AsOf], [RevObjId], [SequenceNumber]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE INDEX [IX_BVSOwner_BVSTranId] ON [Service.BaseValueSegment].[BVSOwner] ([BVSTranId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE INDEX [IX_BVSOwnerValue_BVSValueHeaderId] ON [Service.BaseValueSegment].[BVSOwnerValue] ([BVSValueHeaderId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE INDEX [IX_BVSOwnerValue_BVSOwnerId_DynCalcStepTrackingId] ON [Service.BaseValueSegment].[BVSOwnerValue] ([BVSOwnerId], [DynCalcStepTrackingId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE INDEX [IX_BVSTran_BVSId] ON [Service.BaseValueSegment].[BVSTran] ([BVSId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE INDEX [IX_BVSTran_BVSTranTypeId] ON [Service.BaseValueSegment].[BVSTran] ([BVSTranTypeId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE INDEX [IX_BVSValue_BVSValueHeaderId] ON [Service.BaseValueSegment].[BVSValue] ([BVSValueHeaderId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    CREATE INDEX [IX_BVSValueHeader_BVSTranId] ON [Service.BaseValueSegment].[BVSValueHeader] ([BVSTranId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    ALTER TABLE [Service.BaseValueSegment].[AsmtRevnBVS] ADD CONSTRAINT [FK_AsmtRevnBvs_AsmtRevn] FOREIGN KEY ([AsmtRevnId]) REFERENCES [dbo].[AsmtRevn] ([Id]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170425185408_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170425185408_InitialCreate', N'1.1.1');
END;

GO

