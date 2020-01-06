CREATE TABLE [Service.BaseValueSegment].[BVSOwnerValue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BVSOwnerId] [int] NOT NULL,
	[BVSValueHeaderId] [int] NOT NULL,
	[BaseValue] [decimal](28, 10) NOT NULL,
	[DynCalcStepTrackingId] [int] NOT NULL,
  CONSTRAINT [PK_BVSOwnerValueId] PRIMARY KEY CLUSTERED  ([Id] ASC),
  CONSTRAINT [FK_BVSOwnerValue_BVSOwner] FOREIGN KEY([BVSOwnerId]) REFERENCES [Service.BaseValueSegment].[BVSOwner] ([Id]),
  CONSTRAINT [FK_BVSOwnerValue_BVSValueHeader] FOREIGN KEY([BVSValueHeaderId])REFERENCES [Service.BaseValueSegment].[BVSValueHeader] ([Id])
) 
GO

CREATE INDEX [IX_BVSOwnerValue_BVSOwnerId_DynCalcStepTrackingId] ON [Service.BaseValueSegment].[BVSOwnerValue] ([BVSOwnerId], [DynCalcStepTrackingId])

GO
CREATE INDEX [IX_BVSOwnerValue_BVSValueHeaderId] ON [Service.BaseValueSegment].[BVSOwnerValue] ([BVSValueHeaderId] ASC)
