CREATE TABLE [Service.BaseValueSegment].[BVSOwner](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BVSTranId] [int] NOT NULL,
	[LegalPartyRoleId] [int] NOT NULL,
	[BIPercent] [decimal](28, 10) NOT NULL,
	[DynCalcStepTrackingId] [int] NOT NULL,
 [GRMEventId] INT NULL, 
    CONSTRAINT [PK_BVSOwnerId] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BVSOwner_BVSTran] FOREIGN KEY([BVSTranId]) REFERENCES [Service.BaseValueSegment].[BVSTran] ([Id])
) 
GO
CREATE NONCLUSTERED INDEX [IX_BVSOwner_BVSTranId] ON [Service.BaseValueSegment].[BVSOwner]([BVSTranId])
