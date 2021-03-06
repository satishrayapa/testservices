/*
Script to Create new BVS Service Schema and Tables

Script created by SQL Compare version 12.1.0.3760 from Red Gate Software Ltd at 3/13/2017 11:32:44 AM

*/
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL Serializable
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating schemas'
GO
IF SCHEMA_ID('Service.BaseValueSegment') IS NULL
	EXECUTE('CREATE SCHEMA [Service.BaseValueSegment] AUTHORIZATION [dbo]')
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Service.BaseValueSegment' AND TABLE_NAME = 'BVSTran')
BEGIN 
	PRINT N'Creating [Service.BaseValueSegment].[BVSTRAN]'

	CREATE TABLE [Service.BaseValueSegment].[BVSTran]
	(
	[Id] [int] NOT NULL IDENTITY(1, 1),
	[BVSId] [int] NOT NULL,
	[TranId] [bigint] NOT NULL,
	[EffStatus] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[BVSTranType] [int] NOT NULL,
	[AsmtRevnBVSId] [int] NULL,
	[GRMEventReasonCd] [int] NOT NULL,
	[DynCalcStepTrackingId] [int] NULL
	)

	IF @@ERROR <> 0 SET NOEXEC ON

	PRINT N'Creating primary key [BVSTran0] on [Service.BaseValueSegment].[BVSTran]'

	ALTER TABLE [Service.BaseValueSegment].[BVSTran] ADD CONSTRAINT [BVSTran0] PRIMARY KEY CLUSTERED  ([Id])

	IF @@ERROR <> 0 SET NOEXEC ON
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Service.BaseValueSegment' AND TABLE_NAME = 'BVSOwnerValue')
BEGIN
	PRINT N'Creating [Service.BaseValueSegment].[BVSOwnerValue]'

	CREATE TABLE [Service.BaseValueSegment].[BVSOwnerValue]
	(
	[Id] [int] NOT NULL IDENTITY(1, 1),
	[BVSTranId] [int] NOT NULL,
	[BVSOwnerId] [int] NOT NULL,
	[BVSValueId] [int] NOT NULL,
	[BaseValue] [decimal] (28, 10) NOT NULL,
	[DynCalcStepTrackingId] [int] NOT NULL
	)

	IF @@ERROR <> 0 SET NOEXEC ON

	PRINT N'Creating primary key [PK_BVSOwnerValueID] on [Service.BaseValueSegment].[BVSOwnerValue]'

	ALTER TABLE [Service.BaseValueSegment].[BVSOwnerValue] ADD CONSTRAINT [PK_BVSOwnerValueID] PRIMARY KEY CLUSTERED  ([Id])

	IF @@ERROR <> 0 SET NOEXEC ON
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Service.BaseValueSegment' AND TABLE_NAME = 'BVSValue')
BEGIN
	PRINT N'Creating [Service.BaseValueSegment].[BVSValue]'

	CREATE TABLE [Service.BaseValueSegment].[BVSValue]
	(
	[Id] [int] NOT NULL IDENTITY(1, 1),
	[BVSTranId] [int] NOT NULL,
	[SubComponent] [int] NOT NULL,
	[BaseYear] [smallint] NOT NULL,
	[ValueAmount] [decimal] (28, 10) NOT NULL,
	[PctComplete] [decimal] (14, 10) NOT NULL,
	[FullValueAmount] [decimal] (28, 10) NOT NULL,
	[GRMEventId] [int] NOT NULL,
	[DynCalcStepTrackingId] [int] NOT NULL
	)

	IF @@ERROR <> 0 SET NOEXEC ON

	PRINT N'Creating primary key [BVSValue0] on [Service.BaseValueSegment].[BVSValue]'

	ALTER TABLE [Service.BaseValueSegment].[BVSValue] ADD CONSTRAINT [BVSValue0] PRIMARY KEY CLUSTERED  ([Id])

	IF @@ERROR <> 0 SET NOEXEC ON
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Service.BaseValueSegment' AND TABLE_NAME = 'BVSAsmtEventTran')
BEGIN
	PRINT N'Creating [Service.BaseValueSegment].[BVSAsmtEventTran]'

	CREATE TABLE [Service.BaseValueSegment].[BVSAsmtEventTran]
	(
	[Id] [int] NOT NULL,
	[BVSTranId] [int] NULL,
	[AsmtEventTranId] [int] NULL
	)

	IF @@ERROR <> 0 SET NOEXEC ON

	PRINT N'Creating primary key [PK_BVSAsmtEventTran] on [Service.BaseValueSegment].[BVSAsmtEventTran]'

	ALTER TABLE [Service.BaseValueSegment].[BVSAsmtEventTran] ADD CONSTRAINT [PK_BVSAsmtEventTran] PRIMARY KEY CLUSTERED  ([Id])

	IF @@ERROR <> 0 SET NOEXEC ON
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Service.BaseValueSegment' AND TABLE_NAME = 'BVS')
BEGIN
PRINT N'Creating [Service.BaseValueSegment].[BVS]'

CREATE TABLE [Service.BaseValueSegment].[BVS]
(
[Id] [int] NOT NULL,
[RveobjId] [int] NOT NULL,
[AsOf] [datetime] NOT NULL,
[TranId] [bigint] NOT NULL,
[DynCalcInstanceId] [int] NOT NULL
)

IF @@ERROR <> 0 SET NOEXEC ON

PRINT N'Creating primary key [PK_BVS] on [Service.BaseValueSegment].[BVS]'

ALTER TABLE [Service.BaseValueSegment].[BVS] ADD CONSTRAINT [PK_BVS] PRIMARY KEY CLUSTERED  ([Id])

IF @@ERROR <> 0 SET NOEXEC ON
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Service.BaseValueSegment' AND TABLE_NAME = 'BVSOwner')
BEGIN
	PRINT N'Creating [Service.BaseValueSegment].[BVSOwner]'

	CREATE TABLE [Service.BaseValueSegment].[BVSOwner]
	(
	[Id] [int] NOT NULL IDENTITY(1, 1),
	[BVSTranId] [int] NOT NULL,
	[BeneficialInterest] [int] NOT NULL,
	[Percent] [decimal] (28, 10) NOT NULL,
	[GRMEventId] [int] NOT NULL,
	[DynCalcStepTrackingId] [int] NOT NULL
	)

	IF @@ERROR <> 0 SET NOEXEC ON

	PRINT N'Creating primary key [PK_BVSOwnerID] on [Service.BaseValueSegment].[BVSOwner]'

	ALTER TABLE [Service.BaseValueSegment].[BVSOwner] ADD CONSTRAINT [PK_BVSOwnerID] PRIMARY KEY CLUSTERED  ([Id])

	IF @@ERROR <> 0 SET NOEXEC ON
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_BVSAsmtEventTran_BVSTran')
BEGIN	
	PRINT N'Adding foreign keys to [Service.BaseValueSegment].[BVSAsmtEventTran]'

	ALTER TABLE [Service.BaseValueSegment].[BVSAsmtEventTran] ADD CONSTRAINT [FK_BVSAsmtEventTran_BVSTran] FOREIGN KEY ([BVSTranId]) REFERENCES [Service.BaseValueSegment].[BVSTran] ([Id])
	IF @@ERROR <> 0 SET NOEXEC ON
END 
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_BVSAsmtEventTran_AsmtEventTran')
BEGIN	
	PRINT N'Adding foreign keys to [Service.BaseValueSegment].[BVSAsmtEventTran]'

	ALTER TABLE [Service.BaseValueSegment].[BVSAsmtEventTran] ADD CONSTRAINT [FK_BVSAsmtEventTran_AsmtEventTran] FOREIGN KEY ([AsmtEventTranId]) REFERENCES [dbo].[AsmtEventTran] ([Id])

	IF @@ERROR <> 0 SET NOEXEC ON
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'BVSOwnerValueBVSTran')
BEGIN
	PRINT N'Adding foreign keys to [Service.BaseValueSegment].[BVSOwnerValue]'

	ALTER TABLE [Service.BaseValueSegment].[BVSOwnerValue] ADD CONSTRAINT [BVSOwnerValueBVSTran] FOREIGN KEY ([BVSTranId]) REFERENCES [Service.BaseValueSegment].[BVSTran] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE

	IF @@ERROR <> 0 SET NOEXEC ON
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_BVSValueBVSTran')
BEGIN
	PRINT N'Adding foreign keys to [Service.BaseValueSegment].[BVSOwner]'

	ALTER TABLE [Service.BaseValueSegment].[BVSOwner] ADD CONSTRAINT [FK_BVSValueBVSTran] FOREIGN KEY ([BVSTranId]) REFERENCES [Service.BaseValueSegment].[BVSTran] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE

	IF @@ERROR <> 0 SET NOEXEC ON
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'BVSValueBVSTran')
BEGIN
	PRINT N'Adding foreign keys to [Service.BaseValueSegment].[BVSValue]'

	ALTER TABLE [Service.BaseValueSegment].[BVSValue] ADD CONSTRAINT [BVSValueBVSTran] FOREIGN KEY ([BVSTranId]) REFERENCES [Service.BaseValueSegment].[BVSTran] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE

	IF @@ERROR <> 0 SET NOEXEC ON
END	
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_BVSTran_BVS')
BEGIN
PRINT N'Adding foreign keys to [Service.BaseValueSegment].[BVSTran]'

ALTER TABLE [Service.BaseValueSegment].[BVSTran] ADD CONSTRAINT [FK_BVSTran_BVS] FOREIGN KEY ([BVSId]) REFERENCES [Service.BaseValueSegment].[BVS] ([Id])

IF @@ERROR <> 0 SET NOEXEC ON
END
GO
COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	PRINT 'The database update failed'
END
GO
